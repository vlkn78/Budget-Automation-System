using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjeAyuDeneme.Data;
using ProjeAyuDeneme.Models;
using ProjeAyuDeneme.ViewModels; 

namespace ProjeAyuDeneme.Controllers
{
    [Authorize(Roles = "Admin, Yonetici")]
    public class AdminController : Controller
    {        
        private readonly ApplicationDbContext _context; 
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly RoleManager<IdentityRole> _roleManager; 
        private readonly ILogger<AdminController> _logger; 
        // readonly olarak tanımladık yoksa new ile tanımlamamız gerekecekti, tamamen performans amaçlı. 
        public AdminController(
             ApplicationDbContext context,
             UserManager<ApplicationUser> userManager,
             RoleManager<IdentityRole> roleManager,
             ILogger<AdminController> logger) // controller çalışmadan once bunları once bır calıstırıyoruz.
        {
            _context = context; // veri tabanına artık rahatça erişebiliyoruz. bu sayede sadece veri bağlantısı ile ilgileniyoruz. 
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task<IActionResult> KullaniciListesi() //kulllanıcıları çekiyorum. appuser dırek aktarmak yerıne viewmodel ile 
        {
            var users = _userManager.Users.ToList();
            var userRoleModels = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roleNames = await _userManager.GetRolesAsync(user);

                userRoleModels.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    Email = user.Email ?? "N/A",
                    CurrentRole = roleNames.FirstOrDefault() ?? "Rol Atanmadı",
                     AdSoyad = user.AdSoyad ?? string.Empty,
                    Birim = user.Birim ?? string.Empty,
                    Pozisyon = user.Pozisyon ?? string.Empty
                });
            }
            return View(userRoleModels);
        }

        [HttpGet]
        public async Task<IActionResult> RolDuzenle(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id); // kullanıcıyı bulup getırıyorum. 
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentRole = currentRoles.FirstOrDefault();

            var model = new UserRoleViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                AdSoyad = user.AdSoyad, 
                Birim = user.Birim,   
                Pozisyon = user.Pozisyon, 
                CurrentRole = currentRole ?? "Rol Atanmamış",
                AvailableRoles = roles,
                SelectedRole = currentRole 
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RolDuzenle(UserRoleViewModel model) //Rol eklıyoruz. 
        {
            if (!ModelState.IsValid)
            {
                model.AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }
          
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        
            if (!string.IsNullOrEmpty(model.SelectedRole))
            {
                await _userManager.AddToRoleAsync(user, model.SelectedRole);
            }
            return RedirectToAction("KullaniciListesi");
        }
        [HttpGet]
        public async Task<IActionResult> OnayBekleyenListesi()
        {
            var onayBekleyenListe = new List<OnayBekleyenFaaliyetViewModel>();

            var fuarlar = await _context.Fuarlar
                .Include(f => f.Ofis)
                .Include(f => f.User)
                .Where(f => f.OnayDurumu == "Onay Bekliyor")
                .ToListAsync();

            onayBekleyenListe.AddRange(fuarlar.Select(f => new OnayBekleyenFaaliyetViewModel
            {
                Id = f.Id,
                FaaliyetTuru = "Fuar",
                OfisAdi = f.Ofis?.Ad ?? "N/A", // Ofis null olabilir diye kontrol
                KaydedenKullanici = f.User?.UserName ?? "N/A", // User null olabilir diye kontrol
                Tarih = f.Tarih,
                Degeri = f.Degeri,
                OnayDurumu = f.OnayDurumu,
                DetayLinki = Url.Action("FuarDetay", "Faaliyet", new { id = f.Id }) // Detay linkini oluştur
            }));

            // 2. Onay Bekleyen Ağırlamaları Çek
            var agirlamalar = await _context.Agirlamalar
                .Include(a => a.Ofis)
                .Include(a => a.User)
                .Where(a => a.OnayDurumu == "Onay Bekliyor")
                .ToListAsync();

            onayBekleyenListe.AddRange(agirlamalar.Select(a => new OnayBekleyenFaaliyetViewModel
            {
                Id = a.Id,
                FaaliyetTuru = "Ağırlama",
                OfisAdi = a.Ofis?.Ad ?? "N/A",
                KaydedenKullanici = a.User?.UserName ?? "N/A",
                Tarih = a.Tarih,
                Degeri = a.Degeri,
                OnayDurumu = a.OnayDurumu,
                DetayLinki = Url.Action("AgirlamaDetay", "Faaliyet", new { id = a.Id })
            }));

            var kulturSanatlar = await _context.KulturSanatlar
                .Include(k => k.Ofis)
                .Include(k => k.User)
                .Where(k => k.OnayDurumu == "Onay Bekliyor")
                .ToListAsync();

            onayBekleyenListe.AddRange(kulturSanatlar.Select(k => new OnayBekleyenFaaliyetViewModel
            {
                Id = k.Id,
                FaaliyetTuru = "Kültür-Sanat",
                OfisAdi = k.Ofis?.Ad ?? "N/A",
                KaydedenKullanici = k.User?.UserName ?? "N/A",
                Tarih = k.Tarih,
                Degeri = k.Degeri,
                OnayDurumu = k.OnayDurumu,
                DetayLinki = Url.Action("KulturSanatDetay", "Faaliyet", new { id = k.Id })
            }));
            
            var reklamlar = await _context.Reklamlar
                .Include(r => r.Ofis)
                .Include(r => r.User)
                .Where(r => r.OnayDurumu == "Onay Bekliyor")
                .ToListAsync();

            onayBekleyenListe.AddRange(reklamlar.Select(r => new OnayBekleyenFaaliyetViewModel
            {
                Id = r.Id,
                FaaliyetTuru = "Reklam",
                OfisAdi = r.Ofis?.Ad ?? "N/A",
                KaydedenKullanici = r.User?.UserName ?? "N/A",
                Degeri = r.Degeri,
                OnayDurumu = r.OnayDurumu,
                DetayLinki = Url.Action("ReklamDetay", "Faaliyet", new { id = r.Id })
            }));

            var sektorler = await _context.Sektorler
                .Include(s => s.Ofis)
                .Include(s => s.User)
                .Where(s => s.OnayDurumu == "Onay Bekliyor")
                .ToListAsync();

            onayBekleyenListe.AddRange(sektorler.Select(s => new OnayBekleyenFaaliyetViewModel
            {
                Id = s.Id,
                FaaliyetTuru = "Sektör İşbirliği",
                OfisAdi = s.Ofis?.Ad ?? "N/A",
                KaydedenKullanici = s.User?.UserName ?? "N/A",
                Degeri = s.Degeri,
                OnayDurumu = s.OnayDurumu,
                DetayLinki = Url.Action("SektorDetay", "Faaliyet", new { id = s.Id })
            }));

            var siraliListe = onayBekleyenListe.OrderByDescending(x => x.Tarih).ToList(); 
            return View(siraliListe);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FaaliyetOnayla(int id, string faaliyetTuru)
        {
            bool success = await UpdateOnayDurumuAsync(id, faaliyetTuru, "Onaylandı");

            if (success)
            {
                TempData["SuccessMessage"] = $"{faaliyetTuru} (ID: {id}) başarıyla onaylandı.";
            }
            else
            {
                TempData["ErrorMessage"] = $"{faaliyetTuru} (ID: {id}) onaylanırken bir hata oluştu veya kayıt bulunamadı.";
            }
         
            return RedirectToAction(nameof(OnayBekleyenListesi));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FaaliyetReddet(int id, string faaliyetTuru)
        {
            bool success = await UpdateOnayDurumuAsync(id, faaliyetTuru, "Reddedildi");

            if (success)
            {
                TempData["SuccessMessage"] = $"{faaliyetTuru} (ID: {id}) başarıyla reddedildi.";
            }
            else
            {
                TempData["ErrorMessage"] = $"{faaliyetTuru} (ID: {id}) reddedilirken bir hata oluştu veya kayıt bulunamadı.";
            }

            // Kullanıcıyı onay listesine geri yönlendir
            return RedirectToAction(nameof(OnayBekleyenListesi));        
        }      
        private async Task<bool> UpdateOnayDurumuAsync(int id, string faaliyetTuru, string yeniDurum)
        {           
            try
            {
                switch (faaliyetTuru)
                {
                    case "Fuar":
                        var fuar = await _context.Fuarlar.FindAsync(id);
                        if (fuar == null) return false;
                        fuar.OnayDurumu = yeniDurum;
                        break;
                    case "Ağırlama":
                        var agirlama = await _context.Agirlamalar.FindAsync(id);
                        if (agirlama == null) return false;
                        agirlama.OnayDurumu = yeniDurum;
                        break;
                    case "Kültür-Sanat":
                        var kulturSanat = await _context.KulturSanatlar.FindAsync(id);
                        if (kulturSanat == null) return false;
                        kulturSanat.OnayDurumu = yeniDurum;
                        break;
                    case "Reklam":
                        var reklam = await _context.Reklamlar.FindAsync(id);
                        if (reklam == null) return false;
                        reklam.OnayDurumu = yeniDurum;
                        break;
                    case "Sektör İşbirliği":
                        var sektor = await _context.Sektorler.FindAsync(id);
                        if (sektor == null) return false;
                        sektor.OnayDurumu = yeniDurum;
                        break;
                    default:
                        return false;
                }
               
                await _context.SaveChangesAsync();
                return true; 
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, "Onay durumu güncellenirken hata oluştu. ID: {Id}, Tür: {Tur}", id, faaliyetTuru);
                return false;
            }
        }
    }
}