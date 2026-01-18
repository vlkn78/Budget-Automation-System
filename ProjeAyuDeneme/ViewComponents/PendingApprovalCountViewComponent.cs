using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjeAyuDeneme.Data;
using ProjeAyuDeneme.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProjeAyuDeneme.ViewComponents
{
    public class PendingApprovalCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; // Kullanıcı rolünü kontrol etmek için

        public PendingApprovalCountViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int pendingCount = 0;
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // Kullanıcı giriş yapmışsa ve Admin veya Yonetici rolündeyse sayımı yap
            if (user != null && (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Yonetici")))
            {
                var onayBekliyor = "Onay Bekliyor";

                // Her bir faaliyet tablosundaki onay bekleyen kayıt sayısını al ve topla
                int fuarCount = await _context.Fuarlar.CountAsync(f => f.OnayDurumu == onayBekliyor);
                int agirlamaCount = await _context.Agirlamalar.CountAsync(a => a.OnayDurumu == onayBekliyor);
                int kulturSanatCount = await _context.KulturSanatlar.CountAsync(k => k.OnayDurumu == onayBekliyor);
                int reklamCount = await _context.Reklamlar.CountAsync(r => r.OnayDurumu == onayBekliyor);
                int sektorCount = await _context.Sektorler.CountAsync(s => s.OnayDurumu == onayBekliyor);

                pendingCount = fuarCount + agirlamaCount + kulturSanatCount + reklamCount + sektorCount;
            }

            // Hesaplanan sayıyı View'e gönder
            return View(pendingCount);
        }
    }
}