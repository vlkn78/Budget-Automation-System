using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjeAdin.Models; // NewsModel buradaydý
using ProjeAyuDeneme.Data;
using ProjeAyuDeneme.Models;
using ProjeAyuDeneme.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjeAyuDeneme.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {            
            var viewModel = new AnaSayfaViewModel();
            var currentUser = await _userManager.GetUserAsync(User);
            bool isAdminOrYonetici = User.IsInRole("Admin") || User.IsInRole("Yonetici");
            int currentYear = DateTime.Now.Year;
            var onayBekliyor = "Onay Bekliyor";
            
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://www.tcmb.gov.tr/kurlar/today.xml");
                if (response.IsSuccessStatusCode)
                {
                    var xmlStream = await response.Content.ReadAsStreamAsync();
                    var xmlDoc = XDocument.Load(xmlStream);
                    var istenenKodlar = new List<string> { "USD", "AUD", "DKK", "EUR", "GBP", "CHF", "SEK", "CAD", "KWD", "NOK", "SAR" };
                    viewModel.KurListesi = xmlDoc.Root.Elements("Currency")
                        .Where(c => istenenKodlar.Contains(c.Attribute("Kod").Value))
                        .Select(c => new KurViewModel
                        {
                            Kod = c.Attribute("Kod").Value,
                            Ad = c.Element("Isim").Value,
                            Alis = c.Element("ForexBuying") != null && !string.IsNullOrEmpty(c.Element("ForexBuying").Value) ? decimal.Parse(c.Element("ForexBuying").Value, CultureInfo.InvariantCulture) : 0m,
                            Satis = c.Element("ForexSelling") != null && !string.IsNullOrEmpty(c.Element("ForexSelling").Value) ? decimal.Parse(c.Element("ForexSelling").Value, CultureInfo.InvariantCulture) : 0m
                        }).ToList();
                }
                else { _logger.LogError("TCMB Hata: {StatusCode}", response.StatusCode); viewModel.KurListesi = new List<KurViewModel>(); }
            }
            catch (Exception ex) { _logger.LogError(ex, "TCMB Hata"); viewModel.KurListesi = new List<KurViewModel>(); }

            viewModel.ToplamOfisSayisi = await _context.Ofisler.CountAsync();
            viewModel.ToplamKullaniciSayisi = await _userManager.Users.CountAsync();
            viewModel.BuYilkiToplamHarcama = await _context.Mahsuplar
                                                     .Where(m => m.Tarih.Year == currentYear)
                                                     .SumAsync(m => m.Tutar);
            if (isAdminOrYonetici)
            {
                int fuarCount = await _context.Fuarlar.CountAsync(f => f.OnayDurumu == onayBekliyor);
                int agirlamaCount = await _context.Agirlamalar.CountAsync(a => a.OnayDurumu == onayBekliyor);
                int kulturSanatCount = await _context.KulturSanatlar.CountAsync(k => k.OnayDurumu == onayBekliyor);
                int reklamCount = await _context.Reklamlar.CountAsync(r => r.OnayDurumu == onayBekliyor);
                int sektorCount = await _context.Sektorler.CountAsync(s => s.OnayDurumu == onayBekliyor);
                viewModel.OnayBekleyenSayisi = fuarCount + agirlamaCount + kulturSanatCount + reklamCount + sektorCount;
            }

            viewModel.GonderilenOdenekBuYil = await _context.Odenekler
                                                     .Where(o => o.Tarih.Year == currentYear)
                                                     .SumAsync(o => o.Tutar);
            if (viewModel.GonderilenOdenekBuYil > 0)
            {
                viewModel.HarcamaButceOrani = (int)Math.Round((viewModel.BuYilkiToplamHarcama / viewModel.GonderilenOdenekBuYil) * 100);
            }
            else
            {
                viewModel.HarcamaButceOrani = 0;
            }


            var tertipHarcamaData = await _context.Mahsuplar
                .Where(m => m.Tarih.Year == currentYear && m.Tertip != null)
                .GroupBy(m => m.Tertip)
                .Select(g => new ChartDataPoint { Label = g.Key, Value = g.Sum(m => m.Tutar) })
                .Where(x => x.Value > 0)
                .OrderByDescending(x => x.Value)
                .ToListAsync();

            
            viewModel.TertipHarcamaJson = System.Text.Json.JsonSerializer.Serialize(tertipHarcamaData);

            var ofisHarcamaData = await _context.Mahsuplar
                .Where(m => m.Tarih.Year == currentYear && m.OfisId != null)
                .Include(m => m.Ofis)
                .Where(m => m.Ofis != null)
                .GroupBy(m => m.Ofis.Ad)
                .Select(g => new ChartDataPoint { Label = g.Key, Value = g.Sum(m => m.Tutar) })
                 .Where(x => x.Value > 0)
                .OrderByDescending(x => x.Value)
                .Take(10)
                .ToListAsync();

            
            viewModel.OfisHarcamaJson = System.Text.Json.JsonSerializer.Serialize(ofisHarcamaData);

            if (isAdminOrYonetici)
            {
                var sonFuarlar = await _context.Fuarlar.Include(f => f.Ofis).Where(f => f.OnayDurumu == onayBekliyor).OrderByDescending(f => f.Id).Take(5)
                    .Select(f => new OnayBekleyenFaaliyetViewModel { Id = f.Id, FaaliyetTuru = "Fuar", OfisAdi = f.Ofis.Ad, Tarih = f.Tarih, Degeri = f.Degeri, DetayLinki = Url.Action("FuarDetay", "Faaliyet", new { id = f.Id }) ?? "#" }).ToListAsync();
                var sonAgirlamalar = await _context.Agirlamalar.Include(a => a.Ofis).Where(a => a.OnayDurumu == onayBekliyor).OrderByDescending(a => a.Id).Take(5)
                    .Select(a => new OnayBekleyenFaaliyetViewModel { Id = a.Id, FaaliyetTuru = "Aðýrlama", OfisAdi = a.Ofis.Ad, Tarih = a.Tarih, Degeri = a.Degeri, DetayLinki = Url.Action("AgirlamaDetay", "Faaliyet", new { id = a.Id }) ?? "#" }).ToListAsync();
                var sonKulturSanatlar = await _context.KulturSanatlar.Include(k => k.Ofis).Where(k => k.OnayDurumu == onayBekliyor).OrderByDescending(k => k.Id).Take(5)
                    .Select(k => new OnayBekleyenFaaliyetViewModel { Id = k.Id, FaaliyetTuru = "Kültür-Sanat", OfisAdi = k.Ofis.Ad, Tarih = k.Tarih, Degeri = k.Degeri, DetayLinki = Url.Action("KulturSanatDetay", "Faaliyet", new { id = k.Id }) ?? "#" }).ToListAsync();
                var sonReklamlar = await _context.Reklamlar.Include(r => r.Ofis).Where(r => r.OnayDurumu == onayBekliyor).OrderByDescending(r => r.Id).Take(5)
                    .Select(r => new OnayBekleyenFaaliyetViewModel { Id = r.Id, FaaliyetTuru = "Reklam", OfisAdi = r.Ofis.Ad, Tarih = default, Degeri = r.Degeri, DetayLinki = Url.Action("ReklamDetay", "Faaliyet", new { id = r.Id }) ?? "#" }).ToListAsync();
                var sonSektorler = await _context.Sektorler.Include(s => s.Ofis).Where(s => s.OnayDurumu == onayBekliyor).OrderByDescending(s => s.Id).Take(5)
                    .Select(s => new OnayBekleyenFaaliyetViewModel { Id = s.Id, FaaliyetTuru = "Sektör", OfisAdi = s.Ofis.Ad, Tarih = default, Degeri = s.Degeri, DetayLinki = Url.Action("SektorDetay", "Faaliyet", new { id = s.Id }) ?? "#" }).ToListAsync();

                viewModel.SonOnayBekleyenler = sonFuarlar
                                               .Concat(sonAgirlamalar)
                                               .Concat(sonKulturSanatlar)
                                               .Concat(sonReklamlar)
                                               .Concat(sonSektorler)
                                               .OrderByDescending(x => x.Id)
                                               .Take(5)
                                               .ToList();
            }

            try
            {
                var tumOdenekler = await _context.Odenekler
                    .Include(o => o.Ofis)
                    .Include(o => o.Mahsuplar)
                    .ToListAsync();

                var uyarilar = new List<OdenekUyariViewModel>();

                foreach (var odenek in tumOdenekler)
                {
                    decimal harcananTutar = odenek.Mahsuplar.Sum(m => m.Tutar);
                    decimal kalanTutar = odenek.Tutar - harcananTutar;

                    if (kalanTutar > 0)
                    {
                        DateOnly sonKullanmaTarihi = odenek.Tarih.AddMonths(1);
                        int kalanGun = sonKullanmaTarihi.DayNumber - today.DayNumber;

                        if (kalanGun >= 0 && kalanGun <= 15)
                        {
                            uyarilar.Add(new OdenekUyariViewModel
                            {
                                OdenekId = odenek.Id,
                                OfisAdi = odenek.Ofis?.Ad ?? "N/A",
                                Tertip = odenek.Tertip,
                                OdenekTutari = odenek.Tutar,
                                KalanTutar = kalanTutar,
                                SonKullanmaTarihi = sonKullanmaTarihi,
                                KalanGun = kalanGun,
                                UyariSeviyesi = kalanGun <= 10 ? "kritik" : "normal"
                            });
                        }
                    }
                }
                viewModel.OdenekUyarilari = uyarilar.OrderBy(u => u.KalanGun).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ödenek uyarýlarý hesaplanýrken hata oluþtu.");
                viewModel.OdenekUyarilari = new List<OdenekUyariViewModel>();
            }

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() { return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); }

        public async Task<IActionResult> Haberler(string kaynak = "hepsi")
        {
            string aramaTerimi = "(Turkey OR Türkiye) -thanksgiving -recipe -food -bird -roast -dinner -cooking";
            string zamanFiltresi = "when:30d";
            string kaynakFiltresi = "";

            switch (kaynak)
            {
                case "nytimes": kaynakFiltresi = "+inurl:nytimes.com"; break;
                case "washingtonpost": kaynakFiltresi = "+inurl:washingtonpost.com"; break;              
                case "bbc": kaynakFiltresi = "+inurl:bbc.com"; break;
                case "independent": kaynakFiltresi = "+inurl:independent.co.uk"; break;                
                case "bloomberg": kaynakFiltresi = "+inurl:bloomberg.com"; break;       
                case "aljazeera": kaynakFiltresi = "+inurl:aljazeera.com"; break;
                default: kaynakFiltresi = ""; break;
            }

            ViewBag.SeciliKaynak = kaynak;

            string rssUrl = $"https://news.google.com/rss/search?q={aramaTerimi}{kaynakFiltresi}+{zamanFiltresi}&hl=en-US&gl=US&ceid=US:en";

            List<Article> haberListesi = new List<Article>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                    client.Timeout = TimeSpan.FromSeconds(4);

                    var response = await client.GetStringAsync(rssUrl);
                    XDocument xmlDoc = XDocument.Parse(response);

                    haberListesi = xmlDoc.Descendants("item")
                        .Where(item => item.Element("link") != null
                                       && !item.Element("link").Value.Contains("/video/")
                                       && !item.Element("link").Value.Contains("/watch/"))
                        .Select(item => new Article
                        {
                            Title = item.Element("title")?.Value,
                            Url = item.Element("link")?.Value,
                            PublishedAt = DateTime.TryParse(item.Element("pubDate")?.Value, out DateTime date) ? date : DateTime.Now,
                            Source = new Source { Name = item.Element("source")?.Value ?? "Foreign Press" },
                            Description = "Click to read full coverage...",
                            UrlToImage = "https://images.pexels.com/photos/1550337/pexels-photo-1550337.jpeg?auto=compress&cs=tinysrgb&w=600"
                        })
                        .Take(100)
                        .ToList();
                }
            }
            catch (Exception) { }

            if (haberListesi == null || haberListesi.Count == 0)
            {
                haberListesi = new List<Article>
        {
            new Article { Title = "Turkish markets rally following new economic policy", Source = new Source { Name = "Bloomberg" }, PublishedAt = DateTime.Now.AddDays(-1), Url = "https://www.bloomberg.com/turkey", UrlToImage = "https://images.pexels.com/photos/3760067/pexels-photo-3760067.jpeg?auto=compress&cs=tinysrgb&w=600" },
            new Article { Title = "Istanbul tourism hits all-time high", Source = new Source { Name = "The Independent" }, PublishedAt = DateTime.Now.AddDays(-2), Url = "https://www.independent.co.uk/topic/turkey", UrlToImage = "https://images.pexels.com/photos/2767815/pexels-photo-2767815.jpeg?auto=compress&cs=tinysrgb&w=600" },
            new Article { Title = "Türkiye's strategic role discussed (Global Analysis)", Source = new Source { Name = "BBC News" }, PublishedAt = DateTime.Now.AddDays(-3), Url = "https://www.bbc.com/news/topics/cp7r8vgl2lgt", UrlToImage = "https://images.pexels.com/photos/2048865/pexels-photo-2048865.jpeg?auto=compress&cs=tinysrgb&w=600" },
        };
            }

            return View(haberListesi);
        }
    }
}
