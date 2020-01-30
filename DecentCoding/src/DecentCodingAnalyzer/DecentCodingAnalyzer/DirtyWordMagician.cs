using System;
using System.Collections.Generic;
using System.Linq;

namespace DecentCodingAnalyzer
{
    internal class DirtyWordMagician
    {
        private static Dictionary<string, string> WordDic { get; }

        static DirtyWordMagician()
        {
            WordDic = new Dictionary<string, string>
            {
                { "fuck", "frack" },
                { "shit", "shirt" },
                { "dick", "duck" },
                { "piss", "pass" },
                { "草泥马", "iloveyou" }
            };
        }

        public static bool IsFullDirtyWord(string input)
        {
            return WordDic.Keys.Contains(input.ToLower());
        }

        public static bool HasDirtyWord(string input)
        {
            foreach (var item in WordDic.Keys)
            {
                if (input.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        // TODO: this replace is too low B, refact to use a high performance way
        public static string DirtyToDecent(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                foreach (var kvp in WordDic)
                {
                    // Somefuck -> Somefrack
                    input = input.Replace(kvp.Key, kvp.Value);
                
                    // SomeFuck -> SomeFrack
                    input = input.Replace(kvp.Key.FirstCharToUpper(), kvp.Value.FirstCharToUpper());
                }

                return input;
            }

            return input;
        }
    }
}
