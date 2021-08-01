namespace Rytor.Craps.Microservices.Balanace.Repositories
{
    public class MockBalanceRepository : IBalanceRepository
    {
        private readonly ILogger _logger;
        private readonly string _className = "MockBalanceRepository";

        private List<Models.Account> _accounts = new List<Models.Account> {
            new Models.Account { Id = 1, TwitchId = "rytor", CreateDate = new DateTime(2020, 6, 27, 8, 30, 52) },
            new Models.Account { Id = 1, TwitchId = "MockTwitchUser1", CreateDate = new DateTime(2020, 6, 28, 8, 30, 52) },
            new Models.Account { Id = 1, TwitchId = "MockTwitchUser2", CreateDate = new DateTime(2020, 6, 29, 8, 30, 52) }
        };

        public MockBalanceRepository(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MockBalanceRepository>();
            _logger.LogInformation($"{_className} initialized.");
        }

        public IEnumerable<Models.Balance> GetBalances()
        {
            throw NotImplementedException();
        }
        
        public Models.Balance GetBalanceById(int accountId)
        {
            throw NotImplementedException();
        }
        
        public int CreateBalance(Models.Balance balance)
        {
            throw NotImplementedException();
        }

        public Models.Balance UpdateBalance(Models.Balance balance)
        {
            throw NotImplementedException();
        }

        public bool DeleteBalance(int accountId)
        {
            throw NotImplementedException();
        }
    }
}