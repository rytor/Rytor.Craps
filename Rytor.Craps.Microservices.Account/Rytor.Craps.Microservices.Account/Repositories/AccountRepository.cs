using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Logging;
using Dapper;
using Rytor.Craps.Microservices.Account.Interfaces;
using Models = Rytor.Craps.Microservices.Account.Models;

namespace Rytor.Craps.Microservices.Account.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly string _className = "AccountRepository";

        public AccountRepository(string connectionString, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AccountRepository>();
            _logger.LogInformation("AccountRepository initialized.");
            _connectionString = connectionString;
        }

        public IEnumerable<Models.Account> GetAccounts()
        {
            IEnumerable<Models.Account> result;

            string sql = $@"SELECT Id, TwitchId, CreateDate from dbo.Account";

            try
            {
                _logger.LogDebug($@"{_className}: Getting all Accounts");
                using (var connection = new SqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Account>(sql);
                }

                return result;
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

            string sql = $@"SELECT Id, TwitchId, CreateDate from dbo.Account WHERE Id = {id}";

            try
            {
                _logger.LogDebug($@"{_className}: Getting Account {id}");
                using (var connection = new SqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Account>(sql).First();
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Account {id} - {e.Message}");
                return null;
            }
        }
    }
}