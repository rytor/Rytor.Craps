using System;
using System.Collections.Generic;
using Npgsql;
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
                using (var connection = new NpgsqlConnection(_connectionString))
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
                using (var connection = new NpgsqlConnection(_connectionString))
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

        public int CreateAccount(Models.Account account)
        {
            int newId;

            string sql = $@"INSERT INTO dbo.Account (@twitchId) 
                            VALUES (@TwitchId)
                            SELECT CAST(SCOPE_IDENTITY() as int)";

            try
            {
                _logger.LogDebug($@"{_className}: Creating Account {account.TwitchId}");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    newId = connection.Query<int>(sql, new { twitchId = account.TwitchId }).Single();
                }

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
            string sql = $@"UPDATE dbo.Account
                            SET TwitchId = @twitchId
                            WHERE Id = @id";
            
            try
            {
                _logger.LogDebug($@"{_className}: Updating Account {account.Id} to {account.TwitchId}");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Execute(sql, new { id = account.Id, twitchId = account.TwitchId });
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
            string sql = $@"DELETE FROM dbo.Account
                            WHERE Id = @deleteId";
            
            try
            {
                _logger.LogDebug($@"{_className}: Deleting Account {id}");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Execute(sql, new { deleteId = id });
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