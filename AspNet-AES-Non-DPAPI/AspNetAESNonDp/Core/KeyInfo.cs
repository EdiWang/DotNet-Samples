using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AspNetAESNonDp.Core
{
    public class KeyInfo
    {
        public byte[] Key { get; }
        public byte[] Iv { get; }

        public string KeyString => Convert.ToBase64String(Key);
        public string IVString => Convert.ToBase64String(Iv);

        public KeyInfo()
        {
            using (var myAes = Aes.Create())
            {
                Key = myAes.Key;
                Iv = myAes.IV;
            }
        }

        public KeyInfo(string key, string iv)
        {
            Key = Convert.FromBase64String(key);
            Iv = Convert.FromBase64String(iv);
        }

        public KeyInfo(byte[] key, byte[] iv)
        {
            Key = key;
            Iv = iv;
        }

        public bool HasValues()
        {
            return !string.IsNullOrWhiteSpace(KeyString) && !string.IsNullOrWhiteSpace(IVString);
        }
    }
}
