using System;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{
    // Bu ViewModel, dashboard'da gösterilecek her bir ödenek uyarısının bilgilerini tutar.
    public class OdenekUyariViewModel
    {
        public int OdenekId { get; set; } // Hangi ödeneğe ait olduğu

        [Display(Name = "Ofis Adı")]
        public string OfisAdi { get; set; }

        [Display(Name = "Tertip Kodu")]
        public string Tertip { get; set; }

        [Display(Name = "Ödenek Tutarı")]
        [DataType(DataType.Currency)]
        public decimal OdenekTutari { get; set; }

        [Display(Name = "Kalan Tutar")]
        [DataType(DataType.Currency)]
        public decimal KalanTutar { get; set; }

        [Display(Name = "Son Kullanma Tarihi")]
        [DataType(DataType.Date)]
        public DateOnly SonKullanmaTarihi { get; set; }

        [Display(Name = "Kalan Gün")]
        public int KalanGun { get; set; }

        // Uyarının önem derecesini belirtmek için (Örn: "normal", "kritik")
        public string UyariSeviyesi { get; set; } // Bu, View'de renk belirlemek için kullanılacak
    }
}