using Newtonsoft.Json;

namespace projeAyuDeneme.Models
{
    // Ana API cevabı
    public class GuardianApiResponse
    {
        [JsonProperty("response")]
        public GuardianResponse Response { get; set; }
    }

    // "response" nesnesi
    public class GuardianResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("results")]
        public List<GuardianResult> Results { get; set; } // Makalelerin listesi
    }

    // "results" listesindeki her bir makale
    public class GuardianResult
    {
        [JsonProperty("webTitle")]
        public string WebTitle { get; set; } // Başlık

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; } // Link

        [JsonProperty("webPublicationDate")]
        public DateTime WebPublicationDate { get; set; } // Tarih

        [JsonProperty("fields")]
        public GuardianFields Fields { get; set; } // Ek alanlar (özet için)
    }

    // "fields" nesnesi (özeti içeren)
    public class GuardianFields
    {
        // 'trailText' genellikle özet veya giriş metnini içerir
        [JsonProperty("trailText")]
        public string TrailText { get; set; }
    }
}