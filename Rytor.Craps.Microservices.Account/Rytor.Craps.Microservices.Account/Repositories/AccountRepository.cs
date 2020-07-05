using System;
using System.Collections.Generic;
using Rytor.Craps.Microservices.Account.Interfaces;
using Models = Rytor.Craps.Microservices.Account.Models;

namespace Rytor.Craps.Microservices.Account.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public IEnumerable<Models.Account> GetAccounts()
        {
            return null;
        }

        public Models.Account GetAccountById(int id)
        {
            return null;
        }
    }
}