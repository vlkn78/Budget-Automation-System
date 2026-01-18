using Microsoft.AspNetCore.Mvc.Rendering;
using ProjeAyuDeneme.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{    
    public class HarcamaRaporuViewModel
    {
        [Display(Name = "Ofis Seçiniz")]
        public int? SecilenOfisId { get; set; }

        [Display(Name = "Tertip Kodu Seçiniz")]
        public string? SecilenTertip { get; set; }
        public IEnumerable<SelectListItem> OfisListesi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TertipListesi { get; set; } = new List<SelectListItem>();
        public List<OdenekDetayViewModel> SonucListesi { get; set; } = new List<OdenekDetayViewModel>();
    }
    public class OdenekDetayViewModel
    {
        public int OdenekId { get; set; }
        public DateOnly Tarih { get; set; }
        public string Tertip { get; set; }
        public decimal OdenekTutari { get; set; }
        public decimal HarcamaToplami { get; set; }
        public decimal KalanTutar => OdenekTutari - HarcamaToplami;
        public List<Mahsup> Harcamalar { get; set; } = new List<Mahsup>();
    }
}
