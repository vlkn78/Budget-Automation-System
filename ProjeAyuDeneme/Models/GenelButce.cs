using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjeAyuDeneme.Models
{
    public class GenelButce
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yıl bilgisi zorunludur.")]
        public int Yil { get; set; } // Örn: 2025

        [Required(ErrorMessage = "Tertip kodu zorunludur.")]
        [MaxLength(10)] // Veritabanında kaplayacağı alanı sınırlayalım
        public string Tertip { get; set; } = string.Empty; // Örn: "03.5"

        [Required(ErrorMessage = "Başlangıç tutarı zorunludur.")]
        [Column(TypeName = "decimal(18, 2)")] // Veritabanı tipini açıkça belirtelim
        public decimal BaslangicTutari { get; set; } // Örn: 500000000.00m
    }
}