using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;

namespace AzureAILanguageDetector
{
    public class TextCogResult
    {
        public string Language { get; set; }
        public float Score { get; set; }
        //public bool IsTranslationSupported { get; set; }
        //public bool IsTransliterationSupported { get; set; }
        public Alternative[] Alternatives { get; set; }
    }
}