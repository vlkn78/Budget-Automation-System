namespace ProjeAyuDeneme.ViewModels
{
    // Bu ViewModel, FuarAnaliz sayfasının ihtiyaç duyduğu tüm hesaplanmış verileri tutar.
    public class FuarAnalizViewModel
    {
        public int ToplamFuarSayisi { get; set; }
        public decimal ToplamMaliyet { get; set; }
        public int ToplamKatilimci { get; set; }
        public decimal ToplamStandM2 { get; set; }
        public int ToplamHaberSayisi { get; set; }

        // Hesaplanan Değerler
        public decimal FuarBasinaOrtalamaMaliyet { get; set; }
        public decimal MetrekareBasinaOrtalamaMaliyet { get; set; }
        public double FuarBasinaOrtalamaHaber { get; set; }
    }
}