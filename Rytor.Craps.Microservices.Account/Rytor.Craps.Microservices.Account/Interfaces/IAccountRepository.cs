using System;
using System.Collections.Generic;
using Models = Rytor.Craps.Microservices.Account.Models;

namespace Rytor.Craps.Microservices.Account.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<Models.Account> GetAccounts();
        Models.Account GetAccountById(int id);
    }
}