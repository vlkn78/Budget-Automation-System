using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjeAyuDeneme.Data;
using ProjeAyuDeneme.Models;
using ProjeAyuDeneme.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjeAyuDeneme.Controllers
{
    // Yetkilendirme: Bu Controller'a sadece Admin veya Bütçe Personeli erişebilir
    [Authorize(Roles = "Admin, Yonetici")]
    public class ButceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ButceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GenelButceEkle()
        {
            var viewModel = new GenelButceEkleViewModel
            {
                Yil = DateTime.Now.Year, // Yıl alanını varsayılan olarak mevcut yıl ile doldur
                TertipListesi = GetTertipSelectList() // Yardımcı metodu kullanarak listeyi doldur
            };
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenelButceEkle(GenelButceEkleViewModel viewModel)
        {            
            bool kayitVar = await _context.GenelButceler.AnyAsync(gb => gb.Yil == viewModel.Yil && gb.Tertip == viewModel.Tertip);
            if (kayitVar)
            {
                ModelState.AddModelError("", $"{viewModel.Yil} yılı için {viewModel.Tertip} tertibinde zaten bir başlangıç bütçesi tanımlanmış.");
            }

            if (ModelState.IsValid)
            {
                var yeniGenelButce = new GenelButce
                {
                    Yil = viewModel.Yil,
                    Tertip = viewModel.Tertip,
                    BaslangicTutari = viewModel.BaslangicTutari
                };

                _context.GenelButceler.Add(yeniGenelButce);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"{viewModel.Yil} yılı {viewModel.Tertip} tertibi başlangıç bütçesi başarıyla eklendi.";
                return RedirectToAction(nameof(GenelButceEkle));
            }

            // Hata durumunda dropdown listesini yeniden doldur ve formu tekrar göster.
            viewModel.TertipListesi = GetTertipSelectList();
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> GenelButceRaporu(int? yil)
        {
            int raporYili = yil ?? DateTime.Now.Year;
            var viewModel = new GenelButceRaporuViewModel { SecilenYil = raporYili };

            // 1. Başlangıç Ödenekleri (A)
            var baslangicOdenekleri = await _context.GenelButceler
                                                    .Where(gb => gb.Yil == raporYili)
                                                    .ToDictionaryAsync(gb => gb.Tertip, gb => gb.BaslangicTutari);

            // Eğer seçilen yıl için başlangıç bütçesi girilmemişse, boş rapor göster
            if (!baslangicOdenekleri.Any())
            {
                viewModel.OfisListesi = GetOfislerSelectList();
                viewModel.TertipListesi = GetTertipSelectList();
                return View(viewModel); // Boş RaporSatirlari ile View'e gider
            }

            // 2. Faaliyet Toplamları (B) - Sadece Onaylananlar
            var onaylandiDurumu = "Onaylandı";
            var fuarToplamlari = await _context.Fuarlar.Where(f => f.Tarih.Year == raporYili && f.OnayDurumu == onaylandiDurumu).GroupBy(f => f.Tertip).Select(g => new { Tertip = g.Key, Toplam = g.Sum(f => f.Degeri) }).ToListAsync();
            var agirlamaToplamlari = await _context.Agirlamalar.Where(a => a.Tarih.Year == raporYili && a.OnayDurumu == onaylandiDurumu).GroupBy(a => a.Tertip).Select(g => new { Tertip = g.Key, Toplam = g.Sum(a => a.Degeri) }).ToListAsync();
            var kulturToplamlari = await _context.KulturSanatlar.Where(k => k.Tarih.Year == raporYili && k.OnayDurumu == onaylandiDurumu).GroupBy(k => k.Tertip).Select(g => new { Tertip = g.Key, Toplam = g.Sum(k => k.Degeri) }).ToListAsync();
            var reklamToplamlari = await _context.Reklamlar.Where(r => r.OnayDurumu == onaylandiDurumu).GroupBy(r => r.Tertip).Select(g => new { Tertip = g.Key, Toplam = g.Sum(r => r.Degeri) }).ToListAsync();
            var sektorToplamlari = await _context.Sektorler.Where(s => s.OnayDurumu == onaylandiDurumu).GroupBy(s => s.Tertip).Select(g => new { Tertip = g.Key, Toplam = g.Sum(s => s.Degeri) }).ToListAsync();
            var faaliyetToplamlari = fuarToplamlari.Concat(agirlamaToplamlari).Concat(kulturToplamlari).Concat(reklamToplamlari).Concat(sektorToplamlari)
                                                .GroupBy(x => x.Tertip)
                                                .ToDictionary(g => g.Key, g => g.Sum(x => x.Toplam));

            // 3. Gönderilen Ödenek Toplamları (C)
            var gonderilenOdenekler = await _context.Odenekler
                                                    .Where(o => o.Tarih.Year == raporYili)
                                                    .GroupBy(o => o.Tertip)
                                                    .Select(g => new { Tertip = g.Key, Toplam = g.Sum(o => o.Tutar) })
                                                    .ToDictionaryAsync(g => g.Tertip, g => g.Toplam);

            // 4. Harcama Toplamları (D)
            var harcamaToplamlari = await _context.Mahsuplar
                                                  .Where(m => m.Tarih.Year == raporYili)
                                                  .GroupBy(m => m.Tertip)
                                                  .Select(g => new { Tertip = g.Key, Toplam = g.Sum(m => m.Tutar) })
                                                  .ToDictionaryAsync(g => g.Tertip, g => g.Toplam);

            // 5. Rapor Satırları
            var tertipler = baslangicOdenekleri.Keys.ToList();
            foreach (var tertip in tertipler.OrderBy(t => t))
            {
                var satir = new TertipButceDetayViewModel
                {
                    Tertip = tertip,
                    BaslangicOdenegi = baslangicOdenekleri.GetValueOrDefault(tertip, 0m),
                    FaaliyetToplami = faaliyetToplamlari.GetValueOrDefault(tertip, 0m),
                    GonderilenOdenekToplami = gonderilenOdenekler.GetValueOrDefault(tertip, 0m),
                    HarcamaToplami = harcamaToplamlari.GetValueOrDefault(tertip, 0m)
                };
                viewModel.RaporSatirlari.Add(satir);
            }

            // 6. Genel Toplamlar
            viewModel.GenelToplam.BaslangicOdenegi = viewModel.RaporSatirlari.Sum(s => s.BaslangicOdenegi);
            viewModel.GenelToplam.FaaliyetToplami = viewModel.RaporSatirlari.Sum(s => s.FaaliyetToplami);
            viewModel.GenelToplam.GonderilenOdenekToplami = viewModel.RaporSatirlari.Sum(s => s.GonderilenOdenekToplami);
            viewModel.GenelToplam.HarcamaToplami = viewModel.RaporSatirlari.Sum(s => s.HarcamaToplami);

            // View'deki Yıl Seçim Formu için dropdown listelerini dolduruyoruz
            viewModel.OfisListesi = GetOfislerSelectList();
            viewModel.TertipListesi = GetTertipSelectList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> TertipDetay(string tertip, int yil)
        {
            // Gelen parametrelerin geçerli olup olmadığını kontrol ediyoruz.
            if (string.IsNullOrEmpty(tertip) || yil <= 0)
            {
                return BadRequest("Geçersiz tertip kodu veya yıl.");
            }

            var viewModel = new TertipDetayViewModel
            {
                Tertip = tertip,
                Yil = yil
            };

            var onaylandi = "Onaylandı";
            viewModel.Fuarlar = await _context.Fuarlar.Include(f => f.Ofis)
                .Where(f => f.Tertip == tertip && f.Tarih.Year == yil && f.OnayDurumu == onaylandi)
                .OrderBy(f => f.Tarih).ToListAsync();

            viewModel.Agirlamalar = await _context.Agirlamalar.Include(a => a.Ofis)
                .Where(a => a.Tertip == tertip && a.Tarih.Year == yil && a.OnayDurumu == onaylandi)
                .OrderBy(a => a.Tarih).ToListAsync();

            viewModel.KulturSanatlar = await _context.KulturSanatlar.Include(k => k.Ofis)
                .Where(k => k.Tertip == tertip && k.Tarih.Year == yil && k.OnayDurumu == onaylandi)
                .OrderBy(k => k.Tarih).ToListAsync();

            viewModel.Reklamlar = await _context.Reklamlar.Include(r => r.Ofis)
               .Where(r => r.Tertip == tertip && r.OnayDurumu == onaylandi) // Yıl filtresi yok
               .OrderBy(r => r.Id).ToListAsync();

            viewModel.Sektorler = await _context.Sektorler.Include(s => s.Ofis)
                .Where(s => s.Tertip == tertip && s.OnayDurumu == onaylandi) // Yıl filtresi yok
                .OrderBy(s => s.Id).ToListAsync();

            viewModel.Odenekler = await _context.Odenekler.Include(o => o.Ofis)
                .Where(o => o.Tertip == tertip && o.Tarih.Year == yil)
                .OrderBy(o => o.Tarih).ToListAsync();

            viewModel.Harcamalar = await _context.Mahsuplar.Include(m => m.Ofis).Include(m => m.Odenek)
                .Where(m => m.Tertip == tertip && m.Tarih.Year == yil)
                .OrderBy(m => m.Tarih).ToListAsync();

            viewModel.ToplamFaaliyetTutari = viewModel.Fuarlar.Sum(f => f.Degeri) +
                                             viewModel.Agirlamalar.Sum(a => a.Degeri) +
                                             viewModel.KulturSanatlar.Sum(k => k.Degeri) +
                                             viewModel.Reklamlar.Sum(r => r.Degeri) +
                                             viewModel.Sektorler.Sum(s => s.Degeri);
            viewModel.ToplamOdenekTutari = viewModel.Odenekler.Sum(o => o.Tutar);
            viewModel.ToplamHarcamaTutari = viewModel.Harcamalar.Sum(m => m.Tutar);

            return View(viewModel);
        }
        private IEnumerable<SelectListItem> GetOfislerSelectList()
        {
            return _context.Ofisler.OrderBy(o => o.Ad).Select(o => new SelectListItem
            {
                Text = o.Ad,
                Value = o.Id.ToString()
            }).ToList();
        }
        private IEnumerable<SelectListItem> GetTertipSelectList()
        {
            return new List<SelectListItem>
             {
                 new SelectListItem { Value = "03.2", Text = "03.2" },
                 new SelectListItem { Value = "03.5", Text = "03.5" },
                 new SelectListItem { Value = "03.6", Text = "03.6" },
                 new SelectListItem { Value = "03.7", Text = "03.7" }
             };
        }
    }
}

