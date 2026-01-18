using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjeAyuDeneme.Models
{
    public class Reklam
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Ofis")]
        public int OfisId { get; set; }
        public virtual Ofis Ofis { get; set; } = null!;
        public string Ulke { get; set; } = string.Empty;
        public string Mecra { get; set; } = string.Empty;
        public string Donem { get; set; } = string.Empty;
        public string Icerik { get; set; } = string.Empty;
        public decimal Degeri { get; set; }
        public string Tertip { get; set; } = string.Empty;
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;
        [Required]
        [MaxLength(20)]
        [Display(Name = "Onay Durumu")]
        public string OnayDurumu { get; set; } = "Onay Bekliyor";
    }
}
