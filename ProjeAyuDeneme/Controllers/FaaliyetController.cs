using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjeAyuDeneme.Data;
using ProjeAyuDeneme.Models;
using ProjeAyuDeneme.ViewModels;
using System.Security.Claims;

namespace ProjeAyuDeneme.Controllers
{
    [Authorize(Roles = "Admin, User, Yonetici")]
    public class FaaliyetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FaaliyetController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult FuarEkle()
        {
            var viewModel = new FuarlarViewModel
            {
             
                OfisListesi = GetOfislerSelectList(),
                TertipListesi = GetTertipSelectList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FuarEkle(FuarlarViewModel viewModel)
        {
            if (ModelState.IsValid)
            {              
                var yeniFuar = new Fuar();
                yeniFuar.OfisId = viewModel.OfisId;
                yeniFuar.Ulke = viewModel.Ulke;
                yeniFuar.FuarAd = viewModel.FuarAd;
                yeniFuar.Tarih = viewModel.Tarih;
                yeniFuar.Tertip = viewModel.Tertip;
                yeniFuar.KatilimSayisi = viewModel.KatilimSayisi;
                yeniFuar.StandM2 = viewModel.StandM2;
                yeniFuar.Degeri = viewModel.Degeri;
                yeniFuar.Degerlendirme = viewModel.Degerlendirme;
                yeniFuar.Etkinlik = viewModel.Etkinlik;
                yeniFuar.Statu = viewModel.Statu;
                yeniFuar.Odul = viewModel.Odul;
                yeniFuar.Haber = viewModel.Haber;
                yeniFuar.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                _context.Fuarlar.Add(yeniFuar);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Fuar kaydı başarıyla oluşturuldu.";
                return RedirectToAction("FuarListesi", "Faaliyet");
            }
            viewModel.OfisListesi = GetOfislerSelectList();
            viewModel.TertipListesi = GetTertipSelectList();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> FuarListesi()
        {
            var fuarlar = await _context.Fuarlar
                                           .Include(f => f.Ofis)
                                           .OrderByDescending(f => f.Tarih) // Listeyi tarihe göre sıralamak daha anlamlıdır.
                                           .ToListAsync();
            return View(fuarlar);
        }
        [HttpGet]
        public async Task<IActionResult> FuarDetay(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fuarDetay = await _context.Fuarlar
                                          .Include(f => f.Ofis) 
                                          .Include(f => f.User) 
                                          .FirstOrDefaultAsync(f => f.Id == id);

            if (fuarDetay == null) 
            {
                return NotFound();
            }

            return View(fuarDetay);
        }
        [HttpGet]
        public async Task<IActionResult> FuarAnaliz()
        {
           
            var fuarlar = await _context.Fuarlar.ToListAsync();

            if (!fuarlar.Any()) 
            {
                return View(new FuarAnalizViewModel());
            }
            
            var viewModel = new FuarAnalizViewModel
            {
                ToplamFuarSayisi = fuarlar.Count(),
                ToplamMaliyet = fuarlar.Sum(f => f.Degeri),
                ToplamKatilimci = fuarlar.Sum(f => f.KatilimSayisi),
                ToplamStandM2 = fuarlar.Sum(f => f.StandM2),
                
                ToplamHaberSayisi = fuarlar.Where(f => int.TryParse(f.Haber, out _)).Sum(f => int.Parse(f.Haber)),
                FuarBasinaOrtalamaMaliyet = fuarlar.Average(f => f.Degeri),
                MetrekareBasinaOrtalamaMaliyet = fuarlar.Sum(f => f.StandM2) > 0 ? fuarlar.Sum(f => f.Degeri) / fuarlar.Sum(f => f.StandM2) : 0,
                FuarBasinaOrtalamaHaber = fuarlar.Any(f => int.TryParse(f.Haber, out _)) ? fuarlar.Where(f => int.TryParse(f.Haber, out _)).Average(f => int.Parse(f.Haber)) : 0
            };

            return View(viewModel); 
        }
        
        [HttpGet]
        public IActionResult AgirlamaEkle()
        {
            var viewModel = new AgirlamaEkleViewModel
            {
                Tarih = DateOnly.FromDateTime(DateTime.Today),
                OfisListesi = GetOfislerSelectList(),
                TertipListesi = GetTertipSelectList(),
                SehirListesi = GetSehirSelectList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgirlamaEkle(AgirlamaEkleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var yeniAgirlama = new Agirlama
                {
                    OfisId = viewModel.OfisId,
                    Ulke = viewModel.Ulke,
                    Adet = viewModel.Adet,
                    Kurum = viewModel.Kurum,
                    Tarih = viewModel.Tarih,
                    Sehir = viewModel.Sehir,
                    Kapsam = viewModel.Kapsam,
                    Sonuc = viewModel.Sonuc,
                    Degeri = viewModel.Degeri,
                    Tertip = viewModel.Tertip,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                _context.Agirlamalar.Add(yeniAgirlama);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Ağırlama kaydı başarıyla oluşturuldu.";
                return RedirectToAction("AgirlamaListesi");
            }

            // Hata durumunda dropdown'ları yeniden doldur
            viewModel.OfisListesi = GetOfislerSelectList();
            viewModel.TertipListesi = GetTertipSelectList();
            viewModel.SehirListesi = GetSehirSelectList();
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> AgirlamaListesi() 
        {
            var agirlamalar = await _context.Agirlamalar.Include(a => a.Ofis).OrderByDescending(a => a.Tarih).ToListAsync();
            return View(agirlamalar);
        }
        [HttpGet] 
        public async Task<IActionResult> AgirlamaDetay(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agirlamaDetay = await _context.Agirlamalar
                                              .Include(a => a.Ofis)
                                              .Include(a => a.User)
                                              .FirstOrDefaultAsync(a => a.Id == id);
            if (agirlamaDetay == null)
            {
                return NotFound();
            }

            return View(agirlamaDetay);
        }
        [HttpGet]
        public async Task<IActionResult> AgirlamaAnaliz()
        {
            var agirlamalar = await _context.Agirlamalar.ToListAsync();

            if (!agirlamalar.Any())
            {
                return View(new AgirlamaAnalizViewModel());
            }

            // Eksik hesaplamaları tamamlıyoruz.
            var toplamAgirlananKisi = agirlamalar.Sum(a => a.Adet);
            var toplamMaliyet = agirlamalar.Sum(a => a.Degeri);

            var viewModel = new AgirlamaAnalizViewModel
            {
                ToplamAdet = agirlamalar.Count(),
                // ToplamSehir, farklı şehirlerin sayısını saymalı.
                ToplamSehir = agirlamalar.Select(a => a.Sehir).Distinct().Count(),
                ToplamMaliyet = toplamMaliyet,
                // Hata almamak için 0'a bölünme kontrolü yapıyoruz.
                AgirlamaBasinaMaliyet = agirlamalar.Count() > 0 ? toplamMaliyet / agirlamalar.Count() : 0,
                AgirlamaKisiBasiMaliyet = toplamAgirlananKisi > 0 ? toplamMaliyet / toplamAgirlananKisi : 0
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult KulturSanatEkle()
        {
            // View'in ihtiyaç duyduğu dropdown listelerini içeren boş bir ViewModel oluşturuyoruz.
            var viewModel = new KulturSanatEkleViewModel
            {
                Tarih = DateOnly.FromDateTime(DateTime.Today), // Tarih alanını bugünün tarihiyle doldur
                OfisListesi = GetOfislerSelectList(),
                TertipListesi = GetTertipSelectList()
            };
            return View(viewModel);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KulturSanatEkle(KulturSanatEkleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // 1. ViewModel'dan gelen verileri yeni bir KulturSanat modeline aktar ("map" et).
                var yeniKulturSanat = new KulturSanat
                {
                    OfisId = viewModel.OfisId,
                    Ulke = viewModel.Ulke,
                    Etkinlik = viewModel.Etkinlik,
                    Tarih = viewModel.Tarih,
                    Mekan = viewModel.Mekan,
                    Kurum = viewModel.Kurum,
                    Kisi = viewModel.Kisi,
                    Haber = viewModel.Haber,
                    Degeri = viewModel.Degeri,
                    Tertip = viewModel.Tertip,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) // Giriş yapmış kullanıcının ID'si
                };

                // 2. Hazırlanan nesneyi veritabanına ekle ve kaydet.
                _context.KulturSanatlar.Add(yeniKulturSanat);
                await _context.SaveChangesAsync();

                // 3. Başarı mesajı göster ve kullanıcıyı başka bir sayfaya yönlendir.
                TempData["SuccessMessage"] = "Kültür-Sanat kaydı başarıyla oluşturuldu.";
                return RedirectToAction("KulturSanatListesi", "Faaliyet"); // Veya KulturSanatListesi'ne yönlendirilebilir.
            }

            // Eğer formda bir hata varsa (ModelState.IsValid false ise),
            // kullanıcıya formu, girdiği verileri kaybetmeden ve hata mesajlarıyla birlikte
            // tekrar göstermek için dropdown listelerini yeniden doldur.
            viewModel.OfisListesi = GetOfislerSelectList();
            viewModel.TertipListesi = GetTertipSelectList();

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> KulturSanatListesi()
        {
            var kulturSanatListesi = await _context.KulturSanatlar.Include(k => k.Ofis).OrderByDescending(k => k.Tarih).ToListAsync();
            return View(kulturSanatListesi);
        }        
        [HttpGet]
        public async Task<IActionResult> KulturSanatDetay(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kulturSanatDetay = await _context.KulturSanatlar
                .Include(k => k.Ofis)
                .Include(k => k.User)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kulturSanatDetay == null)
            {
                return NotFound();
            }

            return View(kulturSanatDetay);
        }
      
        [HttpGet]
        public async Task<IActionResult> KulturSanatAnaliz()
        {
            var kulturSanatEtkinlikleri = await _context.KulturSanatlar.ToListAsync();

            if (!kulturSanatEtkinlikleri.Any())
            {
                // Eğer hiç kayıt yoksa, View'in model hatası vermemesi için boş bir ViewModel gönderiyoruz.
                return View(new KulturSanatAnalizViewModel());
            }

            var toplamMaliyet = kulturSanatEtkinlikleri.Sum(k => k.Degeri);
            var toplamKatilimci = kulturSanatEtkinlikleri.Sum(k => k.Kisi);

            var viewModel = new KulturSanatAnalizViewModel
            {
                ToplamEtkinlikSayisi = kulturSanatEtkinlikleri.Count(),
                ToplamMaliyet = toplamMaliyet,
                ToplamKatilimci = toplamKatilimci,
                // Haber alanı string olduğu için, önce sayıya çevirip sonra topluyoruz. Hata almamak için kontrol ekledik.
                ToplamHaberSayisi = kulturSanatEtkinlikleri.Where(k => int.TryParse(k.Haber, out _)).Sum(k => int.Parse(k.Haber)),
                // 0'a bölünme hatasını engellemek için kontrol yapıyoruz.
                EtkinlikBasinaOrtalamaMaliyet = kulturSanatEtkinlikleri.Count() > 0 ? toplamMaliyet / kulturSanatEtkinlikleri.Count() : 0,
                KatilimciBasinaOrtalamaMaliyet = toplamKatilimci > 0 ? toplamMaliyet / toplamKatilimci : 0
            };

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult ReklamEkle()
        {
            var viewModel = new ReklamEkleViewModel
            {
                // View'in ihtiyaç duyduğu dropdown listelerini dolduruyoruz.
                OfisListesi = GetOfislerSelectList(),
                TertipListesi = GetTertipSelectList()
            };
            return View(viewModel);
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReklamEkle(ReklamEkleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // 1. ViewModel'dan gelen verileri yeni bir Reklam modeline aktar.
                var yeniReklam = new Reklam
                {
                    OfisId = viewModel.OfisId,
                    Ulke = viewModel.Ulke,
                    Mecra = viewModel.Mecra,
                    Donem = viewModel.Donem,
                    Icerik = viewModel.Icerik,
                    Degeri = viewModel.Degeri,
                    Tertip = viewModel.Tertip,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) // Giriş yapmış kullanıcının ID'si
                };

                // 2. Hazırlanan nesneyi veritabanına ekle ve kaydet.
                _context.Reklamlar.Add(yeniReklam);
                await _context.SaveChangesAsync();

                // 3. Başarı mesajı göster ve kullanıcıyı başka bir sayfaya yönlendir.
                TempData["SuccessMessage"] = "Reklam kaydı başarıyla oluşturuldu.";
                return RedirectToAction("ReklamListesi", "Faaliyet");
            }

            // Eğer formda bir hata varsa, dropdown listelerini yeniden doldur ve formu tekrar göster.
            viewModel.OfisListesi = GetOfislerSelectList();
            viewModel.TertipListesi = GetTertipSelectList();

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> ReklamListesi()
        {
            // Reklam listesini çekerken, her bir kaydın hangi ofise ait olduğunu
            // görebilmek için .Include(r => r.Ofis) komutunu kullanıyoruz.
            var reklamlar = await _context.Reklamlar
                                          .Include(r => r.Ofis)
                                          .OrderByDescending(r => r.Id) // En yeni kayıtlar en üstte
                                          .ToListAsync();

            return View(reklamlar);
        }
        [HttpGet]
        public async Task<IActionResult> ReklamDetay(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Reklam kaydını çekerken, ilişkili olduğu Ofis ve User (kullanıcı)
            // bilgilerini de .Include() ile birlikte yüklüyoruz.
            var reklamDetay = await _context.Reklamlar
                                          .Include(r => r.Ofis)
                                          .Include(r => r.User)
                                          .FirstOrDefaultAsync(r => r.Id == id);

            if (reklamDetay == null)
            {
                return NotFound();
            }

            return View(reklamDetay);
        }
        [HttpGet]
        public async Task<IActionResult> ReklamAnaliz()
        {
            // Veritabanındaki tüm reklam kayıtlarını çekiyoruz.
            var reklamlar = await _context.Reklamlar.ToListAsync();

            if (!reklamlar.Any())
            {
                return View(new ReklamAnalizViewModel());
            }

            // Gerekli hesaplamaları LINQ'nun Sum ve Count gibi fonksiyonlarıyla yapıyoruz.
            var toplamMaliyet = reklamlar.Sum(r => r.Degeri);

            var viewModel = new ReklamAnalizViewModel
            {
                ToplamReklamSayisi = reklamlar.Count(),
                ToplamMaliyet = toplamMaliyet,
                // 0'a bölünme hatasını engellemek için kontrol yapıyoruz.
                ReklamBasinaOrtalamaMaliyet = reklamlar.Count() > 0 ? toplamMaliyet / reklamlar.Count() : 0
            };

            // İçi dolu ViewModel'ı View'e gönderiyoruz.
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult SektorEkle()
        {
            var viewModel = new SektorEkleViewModel
            {
                // View'in ihtiyaç duyduğu dropdown listelerini dolduruyoruz.
                OfisListesi = GetOfislerSelectList(),
                TertipListesi = GetTertipSelectList()
            };
            return View(viewModel);
        }                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SektorEkle(SektorEkleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // 1. ViewModel'dan gelen verileri yeni bir Sektor modeline aktar.
                var yeniSektor = new Sektor
                {
                    OfisId = viewModel.OfisId,
                    Ulke = viewModel.Ulke,
                    Kurum = viewModel.Kurum,
                    Mecra = viewModel.Mecra,
                    Donem = viewModel.Donem,
                    Degeri = viewModel.Degeri,
                    Kriter = viewModel.Kriter,
                    Tur = viewModel.Tur,
                    Tertip = viewModel.Tertip,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) // Giriş yapmış kullanıcının ID'si
                };

                // 2. Hazırlanan nesneyi veritabanına ekle ve kaydet.
                _context.Sektorler.Add(yeniSektor);
                await _context.SaveChangesAsync();

                // 3. Başarı mesajı göster ve kullanıcıyı başka bir sayfaya yönlendir.
                TempData["SuccessMessage"] = "Sektör işbirliği kaydı başarıyla oluşturuldu.";
                return RedirectToAction("Index", "Home");
            }

            // Eğer formda bir hata varsa, dropdown listelerini yeniden doldur ve formu tekrar göster.
            viewModel.OfisListesi = GetOfislerSelectList();
            viewModel.TertipListesi = GetTertipSelectList();

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> SektorListesi()
        {
            // Sektör listesini çekerken, her bir kaydın hangi ofise ait olduğunu
            // görebilmek için .Include(s => s.Ofis) komutunu kullanıyoruz.
            var sektorler = await _context.Sektorler
                                          .Include(s => s.Ofis)
                                          .OrderByDescending(s => s.Id) // En yeni kayıtlar en üstte
                                          .ToListAsync();

            return View(sektorler);
        }
        [HttpGet]
        public async Task<IActionResult> SektorDetay(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Sektör kaydını çekerken, ilişkili olduğu Ofis ve User (kullanıcı)
            // bilgilerini de .Include() ile birlikte yüklüyoruz.
            var sektorDetay = await _context.Sektorler
                                          .Include(s => s.Ofis)
                                          .Include(s => s.User)
                                          .FirstOrDefaultAsync(s => s.Id == id);

            if (sektorDetay == null)
            {
                return NotFound();
            }

            return View(sektorDetay);
        }
        [HttpGet]
        public async Task<IActionResult> SektorAnaliz()
        {
            // Veritabanındaki tüm sektör kayıtlarını çekiyoruz.
            var sektorKayitlari = await _context.Sektorler.ToListAsync();

            if (!sektorKayitlari.Any())
            {
                // Eğer hiç kayıt yoksa, View'in model hatası vermemesi için
                // içi boş bir ViewModel gönderiyoruz.
                return View(new SektorAnalizViewModel());
            }

            // Gerekli hesaplamaları LINQ'nun Sum ve Count gibi fonksiyonlarıyla yapıyoruz.
            var toplamMaliyet = sektorKayitlari.Sum(s => s.Degeri);

            var viewModel = new SektorAnalizViewModel
            {
                ToplamKayitSayisi = sektorKayitlari.Count(),
                ToplamMaliyet = toplamMaliyet,
                // 0'a bölünme hatasını engellemek için kontrol yapıyoruz.
                KayitBasinaOrtalamaMaliyet = sektorKayitlari.Count() > 0 ? toplamMaliyet / sektorKayitlari.Count() : 0
            };

            // İçi dolu ViewModel'ı View'e gönderiyoruz.
            return View(viewModel);
        }

        private IEnumerable<SelectListItem> GetOfislerSelectList()
        {
            return _context.Ofisler.Select(o => new SelectListItem
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
        private IEnumerable<SelectListItem> GetSehirSelectList()
        {
            var sehirler = new List<string>
            {
                "İstanbul", "Antalya", "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Amasya", "Ankara", "Artvin",
                "Aydın", "Balıkesir", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale",
                "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Edirne", "Elazığ", "Erzincan", "Erzurum",
                "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Isparta", "Mersin",
                "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir", "Kocaeli",
                "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş",
                "Nevşehir", "Niğde", "Ordu", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas",
                "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat", "Zonguldak",
                "Aksaray", "Bayburt", "Karaman", "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan",
                "Iğdır", "Yalova", "Karabük", "Kilis", "Osmaniye", "Düzce"
            };
            return sehirler.Select(s => new SelectListItem { Text = s, Value = s }).ToList();
        }
    }
}
