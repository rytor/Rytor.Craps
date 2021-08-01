using System.Collections.Generic;

namespace Rytor.Craps.Microservices.Balance.Repositories
{
    public interface IActivityRepository
    {
        IEnumerable<Models.Activity> GetActivity();
        IEnumerable<Models.Activity> GetActivityByAccountId(int accountId);
        Models.Activity GetActivityById(int id);
        int CreateActivity(Models.Activity activity);
        Models.Activity UpdateActivity(Models.Activity activity);
        bool DeleteActivity(int activityId);
    }
}