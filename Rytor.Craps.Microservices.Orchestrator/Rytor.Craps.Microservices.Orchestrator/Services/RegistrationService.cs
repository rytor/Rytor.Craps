using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Rytor.Craps.Microservices.Orchestrator.Interfaces;
using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Services
{
    public class RegistrationService : IRegistrationService
    {
        string _accountURL;
        string _balanceURL;
        private readonly ILogger<RegistrationService> _logger;
        private readonly string _className = "RegistrationService";

        public RegistrationService(string accountURL, string balanceURL, ILoggerFactory loggerFactory)
        {
            _accountURL = accountURL;
            _balanceURL = balanceURL;
             _logger = loggerFactory.CreateLogger<RegistrationService>();
            _logger.LogInformation("RegistrationService initialized.");
        }

        public async Task<int> CheckInAccount(string twitchId)
        {
            Account returnedAccount = null;
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(httpClientHandler);
            HttpResponseMessage getResponse = await client.GetAsync(String.Concat(_accountURL, "/account/", twitchId));
            if (getResponse.StatusCode == HttpStatusCode.OK)
            {
                returnedAccount = await getResponse.Content.ReadFromJsonAsync<Account>();
                return returnedAccount.Id;
            }
            else if (getResponse.StatusCode == HttpStatusCode.NoContent)
            {
                Account newAccount = new Account { TwitchId = twitchId };
                HttpResponseMessage postAcctResponse = await client.PostAsJsonAsync(String.Concat(_accountURL, "/account/"), newAccount);
                if (postAcctResponse.StatusCode == HttpStatusCode.Created)
                {
                    int newId = postAcctResponse.Content.ReadFromJsonAsync<int>().Result;
                    Balance newBalance = new Balance { AccountId = newId, CurrentBalance = 100, CurrentFloor = 100 };
                    HttpResponseMessage postBalResponse = await client.PostAsJsonAsync(String.Concat(_balanceURL, "/balance/"), newBalance);
                    if (postBalResponse.StatusCode == HttpStatusCode.Created)
                    {
                        return newId;
                    }
                }
            }

            return 0;
        }
    }
}