using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Dapper;
using Rytor.Craps.Microservices.Game.Models;
using Npgsql;
using System;
using System.Linq;

namespace Rytor.Craps.Microservices.Game.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly string _className = "GameRepository";

        public GameRepository(string connectionString, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GameRepository>();
            _logger.LogInformation("GameRepository initialized.");
            _connectionString = connectionString;
        }

        public IEnumerable<GameEventPayout> GetGameEventPayouts()
        {
            IEnumerable<Models.GameEventPayout> result;

            string sql = $@"SELECT GameEventId, PayoutOddsLeft, PayoutOddsRight from dbo.GameEventPayout";

            try
            {
                _logger.LogDebug($@"{_className}: Getting all GameEventPayouts");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    result = connection.Query<Models.GameEventPayout>(sql);
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting GameEventPayouts - {e.Message}");
                return null;
            }
        }

        public IEnumerable<Bet> GetBets()
        {
            IEnumerable<Models.Bet> result;

            string sql = $@"SELECT Id, AccountId, GameEventId, Amount, BetStatusId, Payout, CreateDate from dbo.Bet ORDER BY CreateDate ASC";

            try
            {
                _logger.LogDebug($@"{_className}: Getting all Bets");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Bet>(sql);
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Bets - {e.Message}");
                return null;
            }
        }

        public Bet GetBetById(int id)
        {
            Models.Bet result;

            string sql = $@"SELECT Id, AccountId, GameEventId, Amount, BetStatusId, Payout, CreateDate from dbo.Bet WHERE Id = @Id";

            try
            {
                _logger.LogDebug($@"{_className}: Getting Bet {id}");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Bet>(sql, new { Id = id }).First();
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Bet {id} - {e.Message}");
                return null;
            }
        }

        public IEnumerable<Bet> GetBetsByStatus(BetStatus status)
        {
            IEnumerable<Models.Bet> result;

            string sql = $@"SELECT Id, AccountId, GameEventId, Amount, BetStatusId, Payout, CreateDate from dbo.Bet WHERE BetStatusId = @Status ORDER BY CreateDate ASC";

            try
            {
                _logger.LogDebug($@"{_className}: Getting all Bets for Status {status}");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Bet>(sql, new { Status = status });
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Bets for Status {status} - {e.Message}");
                return null;
            }
        }

        public IEnumerable<Bet> GetBetsByAccountId(int accountId)
        {
            IEnumerable<Models.Bet> result;

            string sql = $@"SELECT Id, AccountId, GameEventId, Amount, BetStatusId, Payout, CreateDate from dbo.Bet WHERE AccountId = @AccountId ORDER BY CreateDate ASC";

            try
            {
                _logger.LogDebug($@"{_className}: Getting all Bets for Account {accountId}");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Bet>(sql, new { AccountId = accountId });
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Bets for Account {accountId} - {e.Message}");
                return null;
            }
        }

        public int CreateBet(Bet bet)
        {
            int newId;

            string sql = $@"INSERT INTO dbo.Bet (AccountId, GameEventId, Amount, BetStatusId, Payout) 
                            VALUES (@AccountId, @GameEventId, @Amount, @BetStatusId, @Payout)
                            SELECT CAST(SCOPE_IDENTITY() as int)";

            try
            {
                _logger.LogDebug($@"{_className}: Creating Bet for Account {bet.AccountId}");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    newId = connection.Query<int>(sql, new { AccountId = bet.AccountId, GameEventId = bet.GameEventId, Amount = bet.Amount, BetStatusId = bet.BetStatusId, Payout = bet.Payout }).Single();
                }

                return newId;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error creating Bet for Account {bet.AccountId} - {e.Message}");
                return -1;
            }
        }

        public Bet UpdateBet(Bet bet)
        {
            string sql = $@"UPDATE dbo.Bet
                            SET AccountId = @AccountId,
                            GameEventId = @GameEventId,
                            Amount = @Amount,
                            BetStatusId = @BetStatusId,
                            Payout = @Payout
                            WHERE Id = @Id";

            try
            {
                _logger.LogDebug($@"{_className}: Updating Bet {bet.Id} to AccountId {bet.AccountId}, GameEventId {bet.GameEventId}, Amount {bet.Amount}, BetStatusId {bet.BetStatusId}, Payout {bet.Payout}");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Execute(sql, new { Id = bet.Id, AccountId = bet.AccountId, GameEventId = bet.GameEventId, Amount = bet.Amount, BetStatusId = bet.BetStatusId, Payout = bet.Payout });
                }

                return GetBetById(bet.Id);
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error updating Bet {bet.Id} - {e.Message}");
                return null;
            }
        }

        public bool DeleteBet(int betId)
        {
            string sql = $@"DELETE FROM dbo.Bet
                            WHERE Id = @Id";

            try
            {
                _logger.LogDebug($@"{_className}: Deleting Bet {betId}");
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Execute(sql, new { Id = betId });
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error deleting Bet {betId} - {e.Message}");
                return false;
            }
        }
    }
}
