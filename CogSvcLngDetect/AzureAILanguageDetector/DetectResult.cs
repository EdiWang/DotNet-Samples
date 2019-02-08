using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureAILanguageDetector
{
    public class DetectResult
    {
        public string RawJson { get; set; }

        public bool IsSuccess => !RawJson.Contains("\"error\"");

        public string ErrorMessage
        {
            get
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(RawJson);
                return obj.error.message.ToString();
            }
        }

        public DetectResult(string rawJson)
        {
            RawJson = rawJson;
        }

        public List<TextCogResult> ToCogResults()
        {
            return IsSuccess ? JsonConvert.DeserializeObject<List<TextCogResult>>(RawJson) : null;
        }
    }
}