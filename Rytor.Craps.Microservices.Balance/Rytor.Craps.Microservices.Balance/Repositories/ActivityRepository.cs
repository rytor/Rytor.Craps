using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Logging;
using Dapper;
using Models = Rytor.Craps.Microservices.Balance.Models;
using Rytor.Craps.Microservices.Balance.Models;

namespace Rytor.Craps.Microservices.Balance.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly string _className = "ActivityRepository";

        public ActivityRepository(string connectionString, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ActivityRepository>();
            _logger.LogInformation("ActivityRepository initialized.");
            _connectionString = connectionString;
        }
        public IEnumerable<Activity> GetActivity()
        {
            IEnumerable<Models.Activity> result;

            string sql = $@"SELECT Id, AccountId, ActivityTypeId, Amount, CreateDate from dbo.Activity";

            try
            {
                _logger.LogDebug($@"{_className}: Getting all Activity");
                using (var connection = new SqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Activity>(sql);
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Activity - {e.Message}");
                return null;
            }
        }

        public IEnumerable<Activity> GetActivityByAccountId(int accountId)
        {
            IEnumerable<Models.Activity> result;

            string sql = $@"SELECT Id, AccountId, ActivityTypeId, Amount, CreateDate from dbo.Activity WHERE AccountId = @AccountId ORDER BY CreateDate ASC";

            try
            {
                _logger.LogDebug($@"{_className}: Getting all Activity for Account {accountId}");
                using (var connection = new SqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Activity>(sql, new { AccountId = accountId });
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Activity for Account {accountId} - {e.Message}");
                return null;
            }
        }

        public Models.Activity GetActivityById(int id)
        {
            Models.Activity result;

            string sql = $@"SELECT Id, AccountId, ActivityTypeId, Amount, CreateDate from dbo.Activity WHERE Id = @Id";

            try
            {
                _logger.LogDebug($@"{_className}: Getting Activity {id}");
                using (var connection = new SqlConnection(_connectionString))
                {
                    result = connection.Query<Models.Activity>(sql, new { Id = id }).First();
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error getting Activity {id} - {e.Message}");
                return null;
            }
        }

        public int CreateActivity(Models.Activity activity)
        {
            int newId;

            string sql = $@"INSERT INTO dbo.Activity (AccountId, ActivityTypeId, Amount) 
                            VALUES (@AccountId, @ActivityTypeId, @Amount)
                            SELECT CAST(SCOPE_IDENTITY() as int)";

            try
            {
                _logger.LogDebug($@"{_className}: Creating Activity for Account {activity.AccountId}");
                using (var connection = new SqlConnection(_connectionString))
                {
                    newId = connection.Query<int>(sql, new { AccountId = activity.AccountId, ActivityTypeId = activity.ActivityTypeId, Amount = activity.Amount }).Single();
                }

                return newId;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error creating Activity for Account {activity.AccountId} - {e.Message}");
                return -1;
            }
        }

        public Models.Activity UpdateActivity(Models.Activity activity)
        {
            string sql = $@"UPDATE dbo.Activity
                            SET AccountId = @AccountId,
                            ActivityTypeId = @ActivityTypeId,
                            Amount = @Amount
                            WHERE Id = @Id";

            try
            {
                _logger.LogDebug($@"{_className}: Updating Activity {activity.Id} to AccountId {activity.AccountId}, ActivityTypeId {activity.ActivityTypeId}, Amount {activity.Amount}");
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute(sql, new { Id = activity.Id, AccountId = activity.AccountId, ActivityTypeId = activity.ActivityTypeId, Amount = activity.Amount });
                }

                return GetActivityById(activity.Id);
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error updating Activity {activity.Id} - {e.Message}");
                return null;
            }
        }

        public bool DeleteActivity(int activityId)
        {
            string sql = $@"DELETE FROM dbo.Activity
                            WHERE Id = @Id";

            try
            {
                _logger.LogDebug($@"{_className}: Deleting Activity {activityId}");
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute(sql, new { Id = activityId });
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error deleting Activity {activityId} - {e.Message}");
                return false;
            }
        }
    }
}