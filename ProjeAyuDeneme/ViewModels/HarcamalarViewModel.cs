using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{
    public class HarcamaEkleViewModel
    {
        [Required(ErrorMessage = "Lütfen bir ofis seçiniz.")]
        [Display(Name = "Ofis")]
        public int OfisId { get; set; }

        [Required(ErrorMessage = "Lütfen bir tertip seçiniz.")]
        [Display(Name = "Harcama Tertibi")]
        public string Tertip { get; set; }

        [Required(ErrorMessage = "Lütfen bir ödenek seçiniz.")]
        [Display(Name = "Ait Olduğu Ödenek")]
        public int OdenekId { get; set; }

        [Required(ErrorMessage = "Lütfen bir fatura tarihi giriniz.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fatura Tarihi")]
        public DateOnly Tarih { get; set; }

        [Required(ErrorMessage = "Lütfen tutar giriniz.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        [Display(Name = "Fatura Tutarı")]        
        public decimal Tutar { get; set; }
        
        public IEnumerable<SelectListItem> OfisListesi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TertipListesi { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> OdenekListesi { get; set; } = new List<SelectListItem>();
    }
}
