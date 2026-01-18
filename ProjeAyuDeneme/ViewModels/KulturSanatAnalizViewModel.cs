namespace ProjeAyuDeneme.ViewModels
{
    // Bu ViewModel, KulturSanatAnaliz sayfasının ihtiyaç duyduğu tüm hesaplanmış verileri tutar.
    public class KulturSanatAnalizViewModel
    {
        public int ToplamEtkinlikSayisi { get; set; }
        public decimal ToplamMaliyet { get; set; }
        public int ToplamKatilimci { get; set; }
        public int ToplamHaberSayisi { get; set; }

        // Hesaplanan Değerler
        public decimal EtkinlikBasinaOrtalamaMaliyet { get; set; }
        public decimal KatilimciBasinaOrtalamaMaliyet { get; set; }
    }
}
