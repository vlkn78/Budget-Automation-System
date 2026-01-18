using Newtonsoft.Json; // Newtonsoft kütüphanesini ekledik

namespace projeAyuDeneme.Models
{
    // Bu ana sınıf, API'dan gelen tüm cevabı temsil eder
    public class NytApiResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("response")]
        public NytResponse Response { get; set; }
    }

    // "response" nesnesinin içini temsil eder
    public class NytResponse
    {
        [JsonProperty("docs")]
        public List<NytDoc> Docs { get; set; }
    }

    // "docs" listesindeki her bir makaleyi ("document") temsil eder
    public class NytDoc
    {
        [JsonProperty("web_url")]
        public string WebUrl { get; set; } // Makale Linki

        [JsonProperty("snippet")]
        public string Snippet { get; set; } // Makale Özeti

        [JsonProperty("pub_date")]
        public DateTime PubDate { get; set; } // Makale Tarihi

        [JsonProperty("headline")]
        public NytHeadline Headline { get; set; } // Makale Başlığı (iç içe)
    }

    // "headline" nesnesinin içini temsil eder
    public class NytHeadline
    {
        [JsonProperty("main")]
        public string Main { get; set; } // Asıl Başlık
    }
}