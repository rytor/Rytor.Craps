using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Rytor.Craps.Microservices.Orchestrator.Interfaces;
using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Services
{
    public class BalanceService : IBalanceService
    {
        string _balanceURL;
        private readonly ILogger<BalanceService> _logger;
        private readonly string _className = "BalanceService";

        public BalanceService(string balanceURL, ILoggerFactory loggerFactory)
        {
            _balanceURL = balanceURL;
             _logger = loggerFactory.CreateLogger<BalanceService>();
            _logger.LogInformation("BalanceService initialized.");
        }


        public async Task<Balance> GetBalance(int accountId)
        {
            Balance result = new Balance();
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(httpClientHandler);
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpResponseMessage response = await client.GetAsync(String.Concat(_balanceURL, "/balance/", accountId));
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<Balance>();
            }
            return result;
        }
    }
}