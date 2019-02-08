using System;
using System.Globalization;
using System.Linq;
using AzureAILanguageDetector;

namespace LngDetectConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var text1 = "Empower every person and every organization on the planet to achieve more";
            var text2 = "予力地球上每一人、每一组织，成就不凡";

            var dt = new TextLanguageDetector("3d7be7cd961e4072bc4359e07b23d23d");
            var result = dt.DetectAsync(text2).Result;
            if (result.IsSuccess)
            {
                var r = result.ToCogResults();

                var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                var ctr = cultures.FirstOrDefault(c => c.Name == r.First().Language);
                if (ctr != null) Console.WriteLine($"{ctr.EnglishName} - {ctr.NativeName}");
            }
            else
            {
                Console.WriteLine(result.ErrorMessage);
            }
            Console.ReadKey();
        }
    }
}
