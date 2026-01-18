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
    [Authorize(Roles = "Admin")]
    public class OdenekController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OdenekController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var son10Odenek = _context.Odenekler
                              .Include(o => o.Ofis)
                              .OrderByDescending(o => o.Id)
                              .Take(10)
                              .ToList();

            return View(son10Odenek);
        }
        [HttpGet]
        public IActionResult BirimEkle()
        {        
            var tumOfisler = _context.Ofisler.ToList();
            return View(tumOfisler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult BirimEkle(Ofis ofis)
        {
          
            if (ModelState.IsValid)
            {               
                _context.Ofisler.Add(ofis);                
                _context.SaveChanges();
                return RedirectToAction("BirimEkle");
            }
          
            var tumOfisler = _context.Ofisler.ToList();
            return View(tumOfisler);
        }

        [HttpGet]
        public IActionResult BirimGuncelle(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }          
            var ofis = _context.Ofisler.Find(id);

            if (ofis == null)
            {
                return NotFound(); 
            }          
            return View(ofis);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BirimGuncelle(Ofis ofis)
        {
            if (ModelState.IsValid)
            {              
                _context.Ofisler.Update(ofis);
                _context.SaveChanges();               
                return RedirectToAction("BirimEkle");
            }
            return View(ofis);
        }

        [HttpGet]
        public IActionResult OdenekEkle()
        {            
            var viewModel = new OdenekEkleViewModel();
            viewModel.Tarih = DateOnly.FromDateTime(DateTime.Today);
            viewModel.OfisListesi = _context.Ofisler.Select(o => new SelectListItem
            {
                Text = o.Ad,
                Value = o.Id.ToString()
            }).ToList();

            viewModel.TertipListesi = new List<SelectListItem>
                {
                    new SelectListItem { Value = "03.2", Text = "03.2" },
                    new SelectListItem { Value = "03.5", Text = "03.5" },
                    new SelectListItem { Value = "03.6", Text = "03.6" },
                    new SelectListItem { Value = "03.7", Text = "03.7" }
                };
       
            return View(viewModel);       
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult OdenekEkle(OdenekEkleViewModel viewModel)
        {

            if (ModelState.IsValid)
            {            
                var yeniOdenek = new Odenek
                {               
                    OfisId = viewModel.OfisId,
                    Tertip = viewModel.Tertip,
                    Tarih = viewModel.Tarih,
                    Tutar = viewModel.Tutar,             
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
             
                _context.Odenekler.Add(yeniOdenek);
                _context.SaveChanges();
       
                return RedirectToAction("Index");
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
        public IActionResult OdenekDuzenle(int? id)
        {
            var guncelodenek = _context.Odenekler
              .Include(o => o.Ofis) 
              .FirstOrDefault(o => o.Id == id);

            return View(guncelodenek);
        }
        [HttpPost]
        public IActionResult OdenekDuzenle(Odenek odenekGuncelle)
        {
            var mevcutOdenek = _context.Odenekler.Find(odenekGuncelle.Id);           
            mevcutOdenek.Tertip = odenekGuncelle.Tertip;
            mevcutOdenek.Tutar = odenekGuncelle.Tutar;
            _context.SaveChanges();
            return RedirectToAction("Index"); 
        }
    }
}
