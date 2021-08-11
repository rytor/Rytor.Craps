using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Models = Rytor.Craps.Microservices.Balance.Models;

namespace Rytor.Craps.Microservices.Balance.Repositories
{
    public class MockBalanceRepository : IBalanceRepository
    {
        private readonly ILogger _logger;
        private readonly string _className = "MockBalanceRepository";

        private List<Models.Balance> _balances = new List<Models.Balance> {
            new Models.Balance { AccountId = 1, CurrentBalance = 1000, CurrentFloor = 100, CreateDate = new DateTime(2020, 6, 27, 8, 30, 52) },
            new Models.Balance { AccountId = 2, CurrentBalance = 2000, CurrentFloor = 200, CreateDate = new DateTime(2020, 6, 27, 8, 30, 52) },
            new Models.Balance { AccountId = 3, CurrentBalance = 3000, CurrentFloor = 300, CreateDate = new DateTime(2020, 6, 27, 8, 30, 52) }
        };

        public MockBalanceRepository(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MockBalanceRepository>();
            _logger.LogInformation($"{_className} initialized.");
        }

        public IEnumerable<Models.Balance> GetBalances()
        {
            try
            {
                _logger.LogDebug($@"{_className}: Getting all Balances");

                return _balances;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Balances - {e.Message}");
                return null;
            }
        }
        
        public Models.Balance GetBalanceById(int accountId)
        {
            Models.Balance result;

            try
            {
                _logger.LogDebug($@"{_className}: Getting Balance for Account {accountId}");

                result = _balances.Where(x => x.AccountId == accountId).FirstOrDefault();

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Balance for Account {accountId} - {e.Message}");
                return null;
            }
        }
        
        public int CreateBalance(Models.Balance balance)
        {
            if (_balances.Any(x => x.AccountId == balance.AccountId))
            {
                _logger.LogError($@"{_className}: Error creating Balance for Account {balance.AccountId} - account already exists");
                return -1;
            }
            else
            {
                _logger.LogDebug($@"{_className}: Creating Balance for Account {balance.AccountId}");
                _balances.Add(balance);
                return 0;
            }
        }

        public Models.Balance UpdateBalance(Models.Balance balance)
        {
            try
            {
                _logger.LogDebug($@"{_className}: Updating Balance for Account {balance.AccountId}");

                int index = _balances.FindIndex(x => x.AccountId == balance.AccountId);

                if (index > -1)
                {
                    _balances[index].CurrentBalance = balance.CurrentBalance;
                    _balances[index].CurrentFloor = balance.CurrentFloor;
                }

                return GetBalanceById(balance.AccountId);
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error updating Balance for Account {balance.AccountId} - {e.Message}");
                return null;
            }
        }

        public bool DeleteBalance(int accountId)
        {
            try
            {
                _logger.LogDebug($@"{_className}: Deleting Balance for Account {accountId}");
                
                int index = _balances.FindIndex(x => x.AccountId == accountId);

                if (index > -1)
                {
                    _balances.RemoveAt(index);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error deleting Balance for Account {accountId} - {e.Message}");
                return false;
            }
        }
    }
}