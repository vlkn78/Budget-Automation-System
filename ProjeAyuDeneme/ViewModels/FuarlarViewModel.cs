using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{
    public class FuarlarViewModel
    {
        [Required(ErrorMessage = "Lütfen bir ofis seçiniz.")]
        [Display(Name = "Ofis")]
        public int OfisId { get; set; }

        [Required(ErrorMessage = "Lütfen fuar adını giriniz.")]
        [Display(Name = "Fuar Adı")]
        public string FuarAd { get; set; }

        [Display(Name = "Ülke")]
        public string Ulke { get; set; }

        [Required(ErrorMessage = "Lütfen fuar tarihini giriniz.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fuar Tarihi")]
        public DateOnly Tarih { get; set; }

        [Display(Name = "Katılımcı Sayısı")]
        public int KatilimSayisi { get; set; }

        [Display(Name = "Stand Büyüklüğü (m²)")]
        public decimal StandM2 { get; set; }

        [Display(Name = "Yapılan Etkinlikler")]
        public string Etkinlik { get; set; }

        [Display(Name = "Fuar Statüsü")]
        public string Statu { get; set; }

        [Display(Name = "Alınan Ödül")]
        public string Odul { get; set; }

        [Display(Name = "Yapılan Haber Sayısı")]
        public string Haber { get; set; }

        [Display(Name = "Genel Değerlendirme")]
        public string Degerlendirme { get; set; }

        [Display(Name = "Harcama Değeri")]
        public decimal Degeri { get; set; }

        [Required(ErrorMessage = "Lütfen bir tertip seçiniz.")]
        [Display(Name = "Tertip Kodu")]
        public string Tertip { get; set; }
        public IEnumerable<SelectListItem> OfisListesi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TertipListesi { get; set; } = new List<SelectListItem>();


    }
}
