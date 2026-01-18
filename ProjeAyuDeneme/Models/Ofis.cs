using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.Models
{
    public class Ofis
    {
        [Key]
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;

        public virtual ICollection<ApplicationUser> Kullanicilar { get; set; } = new List<ApplicationUser>();

        // Bir Ofis'in birden çok Ödeneği olabilir.
        public virtual ICollection<Odenek> Odenekler { get; set; } = new List<Odenek>();

        // Bir Ofis'in birden çok Mahsubu olabilir.
        public virtual ICollection<Mahsup> Mahsuplar { get; set; } = new List<Mahsup>();
    }
}

