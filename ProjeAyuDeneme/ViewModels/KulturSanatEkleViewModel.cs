using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{
    public class KulturSanatEkleViewModel
    {
        [Required(ErrorMessage = "Lütfen bir ofis seçiniz.")]
        [Display(Name = "Ofis")]
        public int OfisId { get; set; }

        [Display(Name = "Ülke")]
        public string Ulke { get; set; }

        [Required(ErrorMessage = "Lütfen etkinlik adını giriniz.")]
        [Display(Name = "Etkinlik Adı")]
        public string Etkinlik { get; set; }

        [Required(ErrorMessage = "Lütfen bir tarih seçiniz.")]
        [DataType(DataType.Date)]
        [Display(Name = "Tarih")]
        public DateOnly Tarih { get; set; }

        [Display(Name = "Mekan")]
        public string Mekan { get; set; }

        [Display(Name = "İşbirliği Yapılan Kurum")]
        public string Kurum { get; set; }

        [Display(Name = "Katılımcı Sayısı")]
        public int Kisi { get; set; }

        [Display(Name = "Yapılan Haber Sayısı")]
        public string Haber { get; set; }

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