using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureAILanguageDetector
{
    public class TextLanguageDetector
    {
        public string Host { get; } = "https://api.cognitive.microsofttranslator.com";

        public string Route { get; } = "/detect?api-version=3.0";

        public string SubscriptionKey { get; }

        public TextLanguageDetector(string subscriptionKey)
        {
            SubscriptionKey = subscriptionKey;
        }

        public async Task<DetectResult> DetectAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            object[] body = { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(Host + Route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
                var response = await client.SendAsync(request);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return new DetectResult(jsonResponse);
            }
        }
    }
}
