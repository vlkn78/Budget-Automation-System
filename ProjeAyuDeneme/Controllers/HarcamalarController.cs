using Microsoft.AspNetCore.Mvc;
using ProjeAyuDeneme.Data;
using ProjeAyuDeneme.ViewModels;
using ProjeAyuDeneme.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;


namespace ProjeAyuDeneme.Controllers
{
    public class HarcamalarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HarcamalarController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult HarcamaEkle()
        {
            var viewModel = new HarcamaEkleViewModel
            {
                Tarih = DateOnly.FromDateTime(DateTime.Today),
                OfisListesi = GetOfislerSelectList(),
                TertipListesi = GetTertipSelectList()
            };
            return View(viewModel);
        }
               
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HarcamaEkle(HarcamaEkleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var yeniMahsup = new Mahsup();
                yeniMahsup.OfisId = viewModel.OfisId;
                yeniMahsup.OdenekId = viewModel.OdenekId;
                yeniMahsup.Tertip = viewModel.Tertip;
                yeniMahsup.Tarih = viewModel.Tarih;
                yeniMahsup.Tutar = viewModel.Tutar;
                yeniMahsup.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Mahsuplar.Add(yeniMahsup);
                await _context.SaveChangesAsync();
                return RedirectToAction("HarcamaRaporu", "Rapor"); 
            }

            viewModel.OfisListesi = GetOfislerSelectList();
            viewModel.TertipListesi = GetTertipSelectList();
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

        [HttpGet]
        public JsonResult GetOdenekler(int ofisId, string tertip)
        {
            var odenekler = _context.Odenekler
                                    .Where(o => o.OfisId == ofisId && o.Tertip == tertip)
                                    .Select(o => new {
                                        id = o.Id,
                                        text = $"Id: {o.Id} - Tutar: {o.Tutar}"
                                    })
                                    .ToList();
            return Json(odenekler);
        }

        [HttpGet]
        public IActionResult HarcamaDuzenle(int id)
        {
            var harcama = _context.Mahsuplar.Find(id);
            return View(harcama);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HarcamaDuzenle(Mahsup formdanGelenHarcama)
        {
            var veritabanindakiHarcama = await _context.Mahsuplar.FindAsync(formdanGelenHarcama.Id);

            if (veritabanindakiHarcama == null)
            {
                return NotFound();
            }
            veritabanindakiHarcama.Tarih = formdanGelenHarcama.Tarih;
            veritabanindakiHarcama.Tutar = formdanGelenHarcama.Tutar;
            await _context.SaveChangesAsync();
            return RedirectToAction("HarcamaRaporu", "Rapor");
        }
    }
}
