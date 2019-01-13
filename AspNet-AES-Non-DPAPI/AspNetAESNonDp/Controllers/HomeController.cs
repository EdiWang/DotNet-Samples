using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetAESNonDp.Core;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAESNonDp.Controllers
{
    public class HomeController : Controller
    {
        private readonly EncryptionService _encryptionService;

        public HomeController(EncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
            _encryptionService.SetKeyIV("45BLO2yoJkvBwz99kBEMlNkxvL40vUSGaqr/WBu3+Vg=", "Ou3fn+I9SVicGWMLkFEgZQ==");
        }

        public IActionResult Index()
        {
            var str = "Hello";
            var enc = _encryptionService.Encrypt(str);
            var dec = _encryptionService.Decrypt(enc);

            return Content($"str: {str}, enc: {enc}, dec: {dec}");
        }

        public IActionResult Encrypt(string str)
        {
            var enc = _encryptionService.Encrypt(str);
            return Content($"str: {str}, enc: {enc}");
        }

        public IActionResult Decrypt(string enc)
        {
            var dec = _encryptionService.Decrypt(enc);
            return Content($"dec: {dec}");
        }
    }
}
