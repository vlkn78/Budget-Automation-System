using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration; // appsettings.json okumak için
using System.Net.Http; // HttpClient (API isteği) için
using Newtonsoft.Json; // JSON dönüştürme için
using projeAyuDeneme.Models; // NytApiModels için
using System.Linq; // Veri sorgulama (Select) için

namespace projeAyuDeneme.Controllers
{
    public class CrawlResultViewModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Summary { get; set; }
        public string Date { get; set; }
    }

    public class CrawlerController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public CrawlerController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> AfarResults()
        {
            var results = await FetchAfarDataAsync();
            return View(results);
        }
        private async Task<List<CrawlResultViewModel>> FetchAfarDataAsync()
        {
            var resultsList = new List<CrawlResultViewModel>();
            try
            {
                using var playwright = await Playwright.CreateAsync();
                await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
                var page = await browser.NewPageAsync();
                await page.GotoAsync("https://www.afar.com/search?q=Türkiye");

                string articleItemSelector = "li.SearchResultsModule-results-item";
                await page.WaitForSelectorAsync(articleItemSelector);

                var articles = await page.QuerySelectorAllAsync(articleItemSelector);

                foreach (var article in articles)
                {
                    var titleElement = await article.QuerySelectorAsync("div.PagePromo-title > a.Link");
                    var title = titleElement != null ? await titleElement.TextContentAsync() : "Başlık Bulunamadı";
                    var url = titleElement != null ? await titleElement.GetAttributeAsync("href") : "#";

                    if (!url.StartsWith("http")) { url = "https://www.afar.com" + url; }

                    var summaryElement = await article.QuerySelectorAsync("div.PagePromo-description");
                    var summary = summaryElement != null ? await summaryElement.TextContentAsync() : "Özet bulunamadı.";

                    var dateElement = await article.QuerySelectorAsync("div.PagePromo-date");
                    var dateStr = dateElement != null ? await dateElement.TextContentAsync() : "Tarih Yok";

                    resultsList.Add(new CrawlResultViewModel
                    {
                        Title = title.Trim(),
                        Url = url,
                        Summary = summary.Trim(),
                        Date = dateStr.Trim()
                    });
                    if (resultsList.Count >= 10)
                    {
                        break;
                    }
                }
                await browser.CloseAsync();
            }
            catch (Exception ex)
            {
                resultsList.Add(new CrawlResultViewModel { Title = "HATA OLUŞTU!", Summary = ex.Message, Url = "#", Date = "" });
            }
            return resultsList;
        }

        [HttpGet]
        public async Task<IActionResult> NytResults()
        {
            var resultsList = new List<CrawlResultViewModel>();
            try
            {
                // 1. API anahtarını oku
                var apiKey = _configuration["ApiKeys:NytApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new Exception("NYT API Key bulunamadı. appsettings.json dosyasını kontrol edin.");
                }
                var httpClient = _httpClientFactory.CreateClient();
                var apiUrl = $"https://api.nytimes.com/svc/search/v2/articlesearch.json?q=türkiye&api-key={apiKey}";

                var responseString = await httpClient.GetStringAsync(apiUrl);
                var apiResponse = JsonConvert.DeserializeObject<NytApiResponse>(responseString);

                if (apiResponse != null && apiResponse.Status == "OK")
                {
                    resultsList = apiResponse.Response.Docs.Select(doc => new CrawlResultViewModel
                    {
                        Title = doc.Headline.Main,
                        Url = doc.WebUrl,
                        Summary = doc.Snippet,
                        Date = doc.PubDate.ToString("yyyy-MM-dd")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                resultsList.Add(new CrawlResultViewModel { Title = "API HATA OLUŞTU!", Summary = ex.Message, Url = "#", Date = "" });
            }
            return View(resultsList);
        }

        [HttpGet]
        public async Task<IActionResult> WapoResults()
        {
            var resultsList = new List<CrawlResultViewModel>();
            var keywords = new List<string> { "turkey", "türkiye", "turkish" };

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var rssUrl = "http://feeds.washingtonpost.com/rss/world";
                using var stream = await httpClient.GetStreamAsync(rssUrl);
                using var xmlReader = System.Xml.XmlReader.Create(stream);
                var feed = System.ServiceModel.Syndication.SyndicationFeed.Load(xmlReader);

                if (feed != null)
                {
                    foreach (var item in feed.Items)
                    {
                        var title = item.Title?.Text.ToLower() ?? "";
                        var summary = item.Summary?.Text.ToLower() ?? "";

                        // Sadece anahtar kelimeye göre filtrele
                        if (keywords.Any(anahtarKelime => title.Contains(anahtarKelime) || summary.Contains(anahtarKelime)))
                        {
                            resultsList.Add(new CrawlResultViewModel
                            {
                                Title = item.Title?.Text ?? "Başlık Yok",
                                Url = item.Links.FirstOrDefault()?.Uri.ToString() ?? "#",
                                Summary = item.Summary?.Text ?? "Özet Yok",
                                Date = item.PublishDate.ToString("yyyy-MM-dd")
                            });
                        }

                        if (resultsList.Count >= 10)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultsList.Add(new CrawlResultViewModel { Title = "RSS HATA OLUŞTU!", Summary = ex.Message, Url = "#", Date = "" });
            }
            return View(resultsList);
        }

        [HttpGet]
        public async Task<IActionResult> BostonGlobeResults()
        {
            var resultsList = new List<CrawlResultViewModel>();
            var keywords = new List<string> { "turkey", "türkiye", "turkish", "ankara", "istanbul" };

            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
           
                var rssUrl = "https://www.boston.com/tag/world-news/feed/";

                using var stream = await httpClient.GetStreamAsync(rssUrl);
                using var xmlReader = System.Xml.XmlReader.Create(stream, new System.Xml.XmlReaderSettings { Async = true });

                var feed = System.ServiceModel.Syndication.SyndicationFeed.Load(xmlReader);

                if (feed != null)
                {
                    foreach (var item in feed.Items)
                    {
                        var title = item.Title?.Text.ToLower() ?? "";
                        var summaryText = item.Summary?.Text ?? "";
                        var summary = summaryText.ToLower();

                        if (keywords.Any(anahtarKelime => title.Contains(anahtarKelime) || summary.Contains(anahtarKelime)))
                        {
                            var cleanSummary = System.Text.RegularExpressions.Regex.Replace(summaryText, "<.*?>", String.Empty);
                            cleanSummary = cleanSummary.Replace("The post", "").Split("appeared first on")[0].Trim();

                            resultsList.Add(new CrawlResultViewModel
                            {
                                Title = item.Title?.Text ?? "Başlık Yok",
                                Url = item.Links.FirstOrDefault()?.Uri.ToString() ?? "#",
                                Summary = cleanSummary,
                                Date = item.PublishDate.ToString("yyyy-MM-dd")
                            });
                        }
                        if (resultsList.Count >= 10) { break; }
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                resultsList.Add(new CrawlResultViewModel { Title = "RSS HATA OLUŞTU!", Summary = "Boston.com RSS akışı erişimi engelledi (403 Forbidden).", Url = "#", Date = "" });
            }
            catch (Exception ex)
            {
                resultsList.Add(new CrawlResultViewModel { Title = "RSS HATA OLUŞTU!", Summary = ex.Message, Url = "#", Date = "" });
            }
            return View(resultsList);
        }

        [HttpGet]
        public async Task<IActionResult> GuardianResults()
        {
            var resultsList = new List<CrawlResultViewModel>();
            try
            {
                var apiKey = _configuration["ApiKeys:GuardianApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new Exception("Guardian API Key bulunamadı. appsettings.json dosyasını kontrol edin.");
                }
                var httpClient = _httpClientFactory.CreateClient();
                var apiUrl = $"https://content.guardianapis.com/search?q=türkiye&show-fields=trailText&page-size=10&api-key={apiKey}";

                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                var responseString = await httpClient.GetStringAsync(apiUrl);

                var apiResponse = JsonConvert.DeserializeObject<GuardianApiResponse>(responseString);

                if (apiResponse?.Response?.Status == "ok" && apiResponse.Response.Results != null)
                {
                    resultsList = apiResponse.Response.Results.Select(result => new CrawlResultViewModel
                    {
                        Title = result.WebTitle,
                        Url = result.WebUrl,
                        Summary = System.Text.RegularExpressions.Regex.Replace(result.Fields?.TrailText ?? "", "<.*?>", String.Empty).Trim(),
                        Date = result.WebPublicationDate.ToString("yyyy-MM-dd")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                resultsList.Add(new CrawlResultViewModel { Title = "Guardian API HATA!", Summary = ex.Message, Url = "#", Date = "" });
            }

            return View(resultsList);
        }

    }
}