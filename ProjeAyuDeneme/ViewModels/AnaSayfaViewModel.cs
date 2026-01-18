namespace ProjeAyuDeneme.ViewModels
{
    public class AnaSayfaViewModel
    {
        
        public List<KurViewModel> KurListesi { get; set; } = new List<KurViewModel>();
        public int OnayBekleyenSayisi { get; set; }
        public int ToplamOfisSayisi { get; set; }
        public int ToplamKullaniciSayisi { get; set; }
        public decimal BuYilkiToplamHarcama { get; set; }

        // Bilgi Kutusu İçin Veriler
        public decimal GonderilenOdenekBuYil { get; set; }
        public int HarcamaButceOrani { get; set; } // Yüzde olarak (0-100)

        // Grafikler İçin Veriler (JSON'a çevrilecek)
        public string TertipHarcamaJson { get; set; } = "[]"; // Chart.js için { label: 'Tertip', value: Tutar } formatı
        public string OfisHarcamaJson { get; set; } = "[]"; // Chart.js için { label: 'Ofis Adı', value: Tutar } formatı

        // Son Aktiviteler Tablosu İçin Veri
        public List<OnayBekleyenFaaliyetViewModel> SonOnayBekleyenler { get; set; } = new List<OnayBekleyenFaaliyetViewModel>();

        public List<OdenekUyariViewModel> OdenekUyarilari { get; set; } = new List<OdenekUyariViewModel>();
    }

}
