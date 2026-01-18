using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjeAyuDeneme.Models
{
    public class Mahsup
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Tarih { get; set; }
        public decimal Tutar { get; set; }
        public string Tertip { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
       
        public int OdenekId { get; set; }

        [ForeignKey("OdenekId")]
        public virtual Odenek Odenek { get; set; } = null!;

        public int OfisId { get; set; }
        [ForeignKey("OfisId")]
        public virtual Ofis Ofis { get; set; } = null!;
    }
}
