using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Logging;
using Dapper;
using Rytor.Craps.Microservices.Balance.Interfaces;
using Models = Rytor.Craps.Microservices.Balance.Models;

namespace Rytor.Craps.Microservices.Balance.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly string _className = "BalanceRepository";

        public BalanceRepository(string connectionString, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BalanceRepository>();
            _logger.LogInformation("BalanceRepository initialized.");
            _connectionString = connectionString;
        }

        public int CreateBalance(Models.Balance balance)
        {
            int newId;

            string sql = $@"INSERT INTO dbo.Balance (AccountId, CurrentBalance, CurrentFloor) 
                            VALUES (@AccountId, @CurrentBalance, @CurrentFloor);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                _logger.LogDebug($@"{_className}: Creating Balance for Account {balance.AccountId}");
                using (var connection = new SqlConnection(_connectionString))
                {
                    newId = connection.Query<int>(sql, new { AccountId = balance.AccountId, CurrentBalance = balance.CurrentBalance, CurrentFloor = balance.CurrentFloor }).First();
                }

                return newId;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error creating Balance for Account {balance.AccountId} - {e.Message}");
                return -1;
            }
        }

        public bool DeleteBalance(int accountId)
        {
            string sql = $@"DELETE FROM dbo.Balance
                            WHERE AccountId = @AccountId";

            try
            {
                _logger.LogDebug($@"{_className}: Deleting Balance for Account {accountId}");
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute(sql, new { AccountId = accountId });
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error deleting Balance for Account {accountId} - {e.Message}");
                return false;
            }
        }

        public Models.Balance GetBalanceById(int accountId)
        {
            Models.Balance result;

            string sql = $@"SELECT AccountId, CurrentBalance, CurrentFloor, CreateDate from dbo.Balance WHERE AccountId = @AccountId";

            try
            {
                _logger.LogDebug($@"{_className}: Getting Balance for Account {accountId}");
                using (var connection = new SqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Balance>(sql, new { AccountId = accountId }).First();
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Balance for Account {accountId} - {e.Message}");
                return null;
            }
        }

        public IEnumerable<Models.Balance> GetBalances()
        {
            IEnumerable<Models.Balance> result;

            string sql = $@"SELECT AccountId, CurrentBalance, CurrentFloor, CreateDate from dbo.Balance";

            try
            {
                _logger.LogDebug($@"{_className}: Getting all Balances");
                using (var connection = new SqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Balance>(sql);
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Balances - {e.Message}");
                return null;
            }
        }

        public Models.Balance UpdateBalance(Models.Balance balance)
        {
            string sql = $@"UPDATE dbo.Balance
                            SET CurrentBalance = @CurrentBalance,
                            CurrentFloor = @CurrentFloor
                            WHERE AccountId = @AccountId";

            try
            {
                _logger.LogDebug($@"{_className}: Updating Balance for Account {balance.AccountId} to {balance.CurrentBalance}");
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute(sql, new { AccountId = balance.AccountId, CurrentBalance = balance.CurrentBalance, CurrentFloor = balance.CurrentFloor });
                }

                return GetBalanceById(balance.AccountId);
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error updating Balance for Account {balance.AccountId} - {e.Message}");
                return null;
            }
        }
    }
}