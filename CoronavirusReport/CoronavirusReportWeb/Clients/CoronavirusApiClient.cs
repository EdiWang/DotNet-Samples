using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoronavirusReportWeb.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CoronavirusReportWeb.Clients
{
    public interface ICoronavirusApiClient
    {
        Task<IEnumerable<InfectionData>> GetInfectionDataAsync();
    }

    public class CoronavirusApiClient : ICoronavirusApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CoronavirusApiClient> _logger;
        private readonly IOptions<AppSettings> _settings;

        public CoronavirusApiClient(
            HttpClient httpClient, 
            ILogger<CoronavirusApiClient> logger, 
            IOptions<AppSettings> settings)
        {
            _logger = logger;
            _settings = settings;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<InfectionData>> GetInfectionDataAsync()
        {
            var data = await _httpClient.GetStringAsync(_settings.Value.DataSource);
            var apiRes = JsonConvert.DeserializeObject<CoronavirusApiResponse>(data);
            var infectionData = JsonConvert.DeserializeObject<IEnumerable<InfectionData>>(apiRes.Data);

            var infectionDataArray = infectionData as InfectionData[] ?? infectionData.ToArray();
            _logger.LogInformation($"Got {infectionDataArray.Length} records from '{_settings.Value.DataSource}'");

            return infectionDataArray;
        }
    }
}
