using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjeAyuDeneme.Models
{
    public class Odenek
    {
        [Key]
        public int Id { get; set; }

        
        [Required(ErrorMessage = "Lütfen bir tarih seçiniz.")]
        public DateOnly Tarih { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        public decimal Tutar { get; set; }
     
        [Required(ErrorMessage = "Lütfen bir tertip seçiniz.")]
        public string Tertip { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty; 

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        public virtual ICollection<Mahsup> Mahsuplar { get; set; } = new List<Mahsup>();

        [Required(ErrorMessage = "Lütfen bir ofis seçiniz.")]
        public int OfisId { get; set; }

        [ForeignKey("OfisId")]
        public virtual Ofis Ofis { get; set; } = null!;


    }
}
