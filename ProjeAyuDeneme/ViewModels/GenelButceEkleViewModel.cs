using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System; // DateTime için

namespace ProjeAyuDeneme.ViewModels
{
    public class GenelButceEkleViewModel
    {
        [Required(ErrorMessage = "Lütfen yılı giriniz.")]
        [Range(2020, 2099, ErrorMessage = "Lütfen geçerli bir yıl giriniz.")]
        [Display(Name = "Bütçe Yılı")]
        public int Yil { get; set; } = DateTime.Now.Year; // Varsayılan olarak mevcut yıl

        [Required(ErrorMessage = "Lütfen bir tertip seçiniz.")]
        [Display(Name = "Tertip Kodu")]
        public string Tertip { get; set; }

        [Required(ErrorMessage = "Lütfen başlangıç tutarını giriniz.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        [Display(Name = "Başlangıç Tutarı")]
        public decimal BaslangicTutari { get; set; }

        // --- Dropdown Listesi ---
        public IEnumerable<SelectListItem> TertipListesi { get; set; } = new List<SelectListItem>();
    }
}