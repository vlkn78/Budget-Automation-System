using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjeAyuDeneme.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? AdSoyad { get; set; }

        public string? Birim { get; set; }

        public string? Pozisyon { get; set; }

        public int? OfisId { get; set; } // Foreign Key (nullable olabilir, yönetici gibi ofisi olmayanlar için)
        [ForeignKey("OfisId")]
        public virtual Ofis? Ofis { get; set; }

        public virtual ICollection<Agirlama> Agirlamalar { get; set; } = new List<Agirlama>();
        public virtual ICollection<Fuar> Fuarlar { get; set; } = new List<Fuar>();
        public virtual ICollection<KulturSanat> KulturSanatlar { get; set; } = new List<KulturSanat>();
        public virtual ICollection<Reklam> Reklamlar { get; set; } = new List<Reklam>();
        public virtual ICollection<Sektor> Sektorler { get; set; } = new List<Sektor>();

        public virtual ICollection<Mahsup> Mahsuplar { get; set; } = new List<Mahsup>();
        public virtual ICollection<Odenek> Odenekler { get; set; } = new List<Odenek>();
    }
}
