using Microsoft.AspNetCore.Mvc.Rendering; // SelectListItem için gerekli
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System; // DateTime için

namespace ProjeAyuDeneme.ViewModels
{   
    public class GenelButceRaporuViewModel
    {
        [Display(Name = "Rapor Yılı")]
        public int SecilenYil { get; set; } = DateTime.Now.Year; 
        public List<TertipButceDetayViewModel> RaporSatirlari { get; set; } = new List<TertipButceDetayViewModel>();
        public TertipButceDetayViewModel GenelToplam { get; set; } = new TertipButceDetayViewModel { Tertip = "Toplam" };
        public IEnumerable<SelectListItem> OfisListesi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TertipListesi { get; set; } = new List<SelectListItem>();
    }
    public class TertipButceDetayViewModel
    {
        public string Tertip { get; set; }
        public decimal BaslangicOdenegi { get; set; } // A
        public decimal FaaliyetToplami { get; set; }  // B
        public decimal GonderilenOdenekToplami { get; set; } // C
        public decimal HarcamaToplami { get; set; } // D
        public decimal KullanilabilecekOdenek => BaslangicOdenegi - FaaliyetToplami; // A - B
        public decimal BeklenenHarcamalar => GonderilenOdenekToplami - HarcamaToplami; // C - D
    }
}

