using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{
    public class AgirlamaEkleViewModel
    {
        [Required(ErrorMessage = "Lütfen bir ofis seçiniz.")]
        [Display(Name = "Ofis")]
        public int OfisId { get; set; }

        [Display(Name = "Ülke")]
        public string Ulke { get; set; }

        [Display(Name = "Ağırlanan Kişi Sayısı")]
        public int Adet { get; set; }

        [Display(Name = "Ağırlanan Kurum/Kişi")]
        public string Kurum { get; set; }

        [Required(ErrorMessage = "Lütfen bir tarih seçiniz.")]
        [DataType(DataType.Date)]
        [Display(Name = "Tarih")]
        public DateOnly Tarih { get; set; }

        [Display(Name = "Şehir")]
        public string Sehir { get; set; }

        [Display(Name = "Ağırlama Kapsamı")]
        public string Kapsam { get; set; }

        [Display(Name = "Sonuç")]
        public string Sonuc { get; set; }

        [Display(Name = "Harcama Değeri")]
        public decimal Degeri { get; set; }

        [Required(ErrorMessage = "Lütfen bir tertip seçiniz.")]
        [Display(Name = "Tertip Kodu")]
        public string Tertip { get; set; }

        // --- Dropdown Listeleri ---
        public IEnumerable<SelectListItem> OfisListesi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TertipListesi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> SehirListesi { get; set; } = new List<SelectListItem>();
    }
}
