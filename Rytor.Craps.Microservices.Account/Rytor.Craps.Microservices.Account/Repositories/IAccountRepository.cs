using System;
using System.Collections.Generic;
using Models = Rytor.Craps.Microservices.Account.Models;

namespace Rytor.Craps.Microservices.Account.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Models.Account> GetAccounts();
        Models.Account GetAccountById(int id);
        int CreateAccount(Models.Account account);
        Models.Account UpdateAccount(Models.Account account);
        bool DeleteAccount(int id);
    }
}