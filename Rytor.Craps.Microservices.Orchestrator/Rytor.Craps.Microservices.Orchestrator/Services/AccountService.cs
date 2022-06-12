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
    public class AccountService : IAccountService
    {
        private readonly string _accountURL;
        private readonly string _balanceURL;
        private readonly ILogger<AccountService> _logger;
        private readonly string _className = "AccountService";

        public AccountService(string accountURL, string balanceURL, ILoggerFactory loggerFactory)
        {
            _accountURL = accountURL;
            _balanceURL = balanceURL;
             _logger = loggerFactory.CreateLogger<AccountService>();
            _logger.LogInformation("AccountService initialized.");
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
                    // TO-DO: Set up event system to automatically create this
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

        public async Task<List<Account>> GetAccounts()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(httpClientHandler);
            HttpResponseMessage getResponse = await client.GetAsync(String.Concat(_accountURL, "/account"));
            if (getResponse.StatusCode == HttpStatusCode.OK)
            {
                var result = await getResponse.Content.ReadFromJsonAsync<List<Account>>();
                return result;
            }
            else
                return null;
        }
    }
}