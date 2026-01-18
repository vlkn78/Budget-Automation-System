using ProjeAyuDeneme.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{
   
    public class TertipDetayViewModel
    {
        [Display(Name = "Tertip Kodu")]
        public string Tertip { get; set; }
        [Display(Name = "Yıl")]
        public int Yil { get; set; }
        public List<Fuar> Fuarlar { get; set; } = new List<Fuar>();
        public List<Agirlama> Agirlamalar { get; set; } = new List<Agirlama>();
        public List<KulturSanat> KulturSanatlar { get; set; } = new List<KulturSanat>();
        public List<Reklam> Reklamlar { get; set; } = new List<Reklam>();
        public List<Sektor> Sektorler { get; set; } = new List<Sektor>();
        public List<Odenek> Odenekler { get; set; } = new List<Odenek>();
        public List<Mahsup> Harcamalar { get; set; } = new List<Mahsup>();
        public decimal ToplamFaaliyetTutari { get; set; }
        public decimal ToplamOdenekTutari { get; set; }
        public decimal ToplamHarcamaTutari { get; set; }

    }
}
