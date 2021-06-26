using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Dapper;
using Rytor.Craps.Microservices.Game.Models;
using System.Data.SqlClient;
using System;

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
                using (var connection = new SqlConnection(_connectionString))
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
    }
}