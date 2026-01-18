using System;
using System.ComponentModel.DataAnnotations;

namespace ProjeAyuDeneme.ViewModels
{
    // Bu ViewModel, yöneticinin onay bekleyen faaliyetler listesinde göreceği
    // her bir satırı temsil eder. Farklı faaliyet türlerinden ortak bilgileri içerir.
    public class OnayBekleyenFaaliyetViewModel
    {
        public int Id { get; set; } // Faaliyetin kendi ID'si (Fuar.Id, Agirlama.Id vb.)

        [Display(Name = "Faaliyet Türü")]
        public string FaaliyetTuru { get; set; } // "Fuar", "Ağırlama", "Kültür-Sanat" vb.

        [Display(Name = "Ofis Adı")]
        public string OfisAdi { get; set; }

        [Display(Name = "Giriş Yapan Kullanıcı")]
        public string KaydedenKullanici { get; set; } // Kullanıcının UserName veya Email'i

        [Display(Name = "Faaliyet Tarihi")]
        [DataType(DataType.Date)]
        public DateOnly Tarih { get; set; }

        [Display(Name = "Faaliyet Değeri")]
        [DataType(DataType.Currency)]
        public decimal Degeri { get; set; }

        [Display(Name = "Mevcut Durum")]
        public string OnayDurumu { get; set; } // Bu listede her zaman "Onay Bekliyor" olacak

        // Controller'da oluşturulacak detay sayfasına giden link (örn: /Faaliyet/FuarDetay/5)
        public string DetayLinki { get; set; }
    }
}