using Microsoft.AspNetCore.Mvc.Rendering;
using ProjeAyuDeneme.Models;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{
    public class OdenekEkleViewModel
    {
        [Required(ErrorMessage = "Lütfen bir ofis seçiniz.")]
        [Display(Name = "Ofis")]
        public int OfisId { get; set; }

        [Required(ErrorMessage = "Lütfen bir tertip seçiniz.")]
        [Display(Name = "Tertip Kodu")]
        public string Tertip { get; set; }

        [Required(ErrorMessage = "Lütfen bir tarih seçiniz.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ödenek Tarihi")]
        public DateOnly Tarih { get; set; }

        [Required(ErrorMessage = "Lütfen tutar giriniz.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        [Display(Name = "Tutar")]
        public decimal Tutar { get; set; }

        // --- Dropdown listeleri için ---
        public IEnumerable<SelectListItem> OfisListesi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TertipListesi { get; set; } = new List<SelectListItem>();
    }
}
