using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjeAyuDeneme.Data;
using ProjeAyuDeneme.ViewModels;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ProjeAyuDeneme.Controllers
{
    public class RaporController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RaporController(ApplicationDbContext context)
        {
            _context = context;
        }       
        [HttpGet]
        public IActionResult HarcamaRaporu()
        {
            var viewModel = new HarcamaRaporuViewModel
            {
               
                OfisListesi = GetOfislerSelectList(),
                TertipListesi = GetTertipSelectList()
            };
            return View(viewModel);
        }        
        [HttpPost]
        public async Task<IActionResult> HarcamaRaporu(HarcamaRaporuViewModel viewModel)
        {
            if (viewModel.SecilenOfisId.HasValue && !string.IsNullOrEmpty(viewModel.SecilenTertip))
            {                
                var odenekler = await _context.Odenekler
                    .Include(o => o.Mahsuplar) 
                    .Where(o => o.OfisId == viewModel.SecilenOfisId && o.Tertip == viewModel.SecilenTertip)
                    .OrderBy(o => o.Tarih)
                    .ToListAsync();
               
                viewModel.SonucListesi = odenekler.Select(o => new OdenekDetayViewModel
                {
                    OdenekId = o.Id,
                    Tarih = o.Tarih,
                    Tertip = o.Tertip,
                    OdenekTutari = o.Tutar,
                    HarcamaToplami = o.Mahsuplar.Sum(m => m.Tutar),
                    Harcamalar = o.Mahsuplar.ToList()
                }).ToList();
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
    }
}