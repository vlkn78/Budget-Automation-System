using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{
    public class SektorEkleViewModel
    {
        [Required(ErrorMessage = "Lütfen bir ofis seçiniz.")]
        [Display(Name = "Ofis")]
        public int OfisId { get; set; }

        [Display(Name = "Ülke")]
        public string Ulke { get; set; }

        [Display(Name = "İşbirliği Yapılan Kurum")]
        public string Kurum { get; set; }

        [Display(Name = "Mecra")]
        public string Mecra { get; set; }

        [Display(Name = "Dönem")]
        public string Donem { get; set; }

        [Display(Name = "Harcama Değeri")]
        public decimal Degeri { get; set; }

        [Display(Name = "İşbirliği Kriteri")]
        public string Kriter { get; set; }

        [Display(Name = "İşbirliği Türü")]
        public string Tur { get; set; }

        [Required(ErrorMessage = "Lütfen bir tertip seçiniz.")]
        [Display(Name = "Tertip Kodu")]
        public string Tertip { get; set; }

        // --- Dropdown Listeleri ---
        public IEnumerable<SelectListItem> OfisListesi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TertipListesi { get; set; } = new List<SelectListItem>();
    }
}