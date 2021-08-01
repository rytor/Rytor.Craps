using System;
using System.Collections.Generic;
using Models = Rytor.Craps.Microservices.Balance.Models;

namespace Rytor.Craps.Microservices.Balance.Repositories
{
    public interface IBalanceRepository
    {
        IEnumerable<Models.Balance> GetBalances();
        Models.Balance GetBalanceById(int accountId);
        int CreateBalance(Models.Balance balance);
        Models.Balance UpdateBalance(Models.Balance balance);
        bool DeleteBalance(int accountId);
    }
}