using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Models = Rytor.Craps.Microservices.Account.Models;

namespace Rytor.Craps.Microservices.Account.Repositories
{
    public class MockAccountRepository : IAccountRepository
    {
        private readonly ILogger _logger;
        private readonly string _className = "MockAccountRepository";

        private List<Models.Account> _accounts = new List<Models.Account> {
            new Models.Account { Id = 1, TwitchId = "rytor", CreateDate = new DateTime(2020, 6, 27, 8, 30, 52) },
            new Models.Account { Id = 2, TwitchId = "MockTwitchUser1", CreateDate = new DateTime(2020, 6, 28, 8, 30, 52) },
            new Models.Account { Id = 3, TwitchId = "MockTwitchUser2", CreateDate = new DateTime(2020, 6, 29, 8, 30, 52) }
        };

        public MockAccountRepository(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MockAccountRepository>();
            _logger.LogInformation("MockAccountRepository initialized.");
        }

        public IEnumerable<Models.Account> GetAccounts()
        {
            try
            {
                _logger.LogDebug($@"{_className}: Getting all Accounts");

                return _accounts;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Accounts - {e.Message}");
                return null;
            }
        }

        public Models.Account GetAccountById(int id)
        {
            Models.Account result;

            try
            {
                _logger.LogDebug($@"{_className}: Getting Account {id}");

                result = _accounts.Where(x => x.Id == id).FirstOrDefault();

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Account {id} - {e.Message}");
                return null;
            }
        }

        public int CreateAccount(Models.Account account)
        {
            int newId;

            try
            {
                _logger.LogDebug($@"{_className}: Creating Account {account.TwitchId}");

                newId = _accounts.Count() + 1;
                account.Id = newId;

                _accounts.Add(account);

                return newId;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error creating Account {account.TwitchId} - {e.Message}");
                return -1;
            }
        }

        public Models.Account UpdateAccount(Models.Account account)
        {            
            try
            {
                _logger.LogDebug($@"{_className}: Updating Account {account.Id} to {account.TwitchId}");

                int index = _accounts.FindIndex(x => x.Id == account.Id);

                if (index > -1)
                {
                    _accounts[index].TwitchId = account.TwitchId;
                }

                return GetAccountById(account.Id);
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error updating Account {account.Id} - {e.Message}");
                return null;
            }
        }

        public bool DeleteAccount(int id)
        {            
            try
            {
                _logger.LogDebug($@"{_className}: Deleting Account {id}");
                
                int index = _accounts.FindIndex(x => x.Id == id);

                if (index > -1)
                {
                    _accounts.RemoveAt(index);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error deleting Account {id} - {e.Message}");
                return false;
            }
        }
    }
}