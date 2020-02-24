using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronavirusReportWeb.Clients;
using CoronavirusReportWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoronavirusReportWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectionDataController : ControllerBase
    {
        private readonly ILogger<InfectionDataController> _logger;

        public InfectionDataController(ILogger<InfectionDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<InfectionData>> Get([FromServices] ICoronavirusApiClient coronavirusApiClient)
        {
            var data = await coronavirusApiClient.GetInfectionDataAsync();
            return data.OrderByDescending(d => d.Date);
        }
    }
}
