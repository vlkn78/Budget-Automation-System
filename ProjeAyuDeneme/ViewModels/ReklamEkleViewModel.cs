using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{
    public class ReklamEkleViewModel
    {
        [Required(ErrorMessage = "Lütfen bir ofis seçiniz.")]
        [Display(Name = "Ofis")]
        public int OfisId { get; set; }

        [Display(Name = "Ülke")]
        public string Ulke { get; set; }

        [Required(ErrorMessage = "Lütfen mecra türünü giriniz.")]
        [Display(Name = "Mecra")]
        public string Mecra { get; set; }

        [Display(Name = "Dönem (Örn: Ocak-Mart 2025)")]
        public string Donem { get; set; }

        [Display(Name = "Reklam İçeriği")]
        public string Icerik { get; set; }

        [Display(Name = "Harcama Değeri")]
        public decimal Degeri { get; set; }

        [Required(ErrorMessage = "Lütfen bir tertip seçiniz.")]
        [Display(Name = "Tertip Kodu")]
        public string Tertip { get; set; }

        // --- Dropdown Listeleri ---
        public IEnumerable<SelectListItem> OfisListesi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TertipListesi { get; set; } = new List<SelectListItem>();
    }
}