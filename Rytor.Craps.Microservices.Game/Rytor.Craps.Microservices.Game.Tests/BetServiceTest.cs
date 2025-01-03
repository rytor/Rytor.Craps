using Rytor.Libraries.Dice;
using Rytor.Craps.Microservices.Game.Models;
using Rytor.Craps.Microservices.Game.Services;
using Rytor.Craps.Microservices.Game.Repositories;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Rytor.Craps.Microservices.Game.Tests
{
    public class BetServiceTest
    {
        private readonly IBetService _betService;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IGameRepository _gameRepository;

        public BetServiceTest()
        {
            var gameEventPayouts = new List<GameEventPayout>
            {
                new GameEventPayout { GameEventId = GameEvent.Pass, PayoutOddsLeft = 1, PayoutOddsRight = 1 },
                new GameEventPayout { GameEventId = GameEvent.Field, PayoutOddsLeft = 1, PayoutOddsRight = 1 }
            };

            var gameRepositoryMock = new Mock<IGameRepository>();
            gameRepositoryMock.Setup(repo => repo.GetGameEventPayouts()).Returns(gameEventPayouts);
            _loggerFactory = new LoggerFactory();
            _betService = new BetService(_loggerFactory, gameRepositoryMock.Object);
        }

        // Add your test methods here
        [Fact]
        public void CalculateBetPayout_PassLine_ReturnsCorrectPayout()
        {
            Bet bet = new Bet {
                Amount = 10,
                BetStatusId = BetStatus.Won,
                GameEventId = GameEvent.Pass,
                Id = 1,
                Payout = 0,
                CreateDate = System.DateTime.Now
            };

            int payout = _betService.CalculateBetPayout(bet);

            Assert.Equal(10, payout);
        }

        [Fact]
        public void CalculateBetPayout_Field_NoMultiplier_ReturnsCorrectPayout()
        {
            Bet bet = new Bet {
                Amount = 10,
                BetStatusId = BetStatus.Won,
                GameEventId = GameEvent.Field,
                Id = 1,
                Payout = 0,
                CreateDate = System.DateTime.Now
            };

            int payout = _betService.CalculateBetPayout(bet);

            Assert.Equal(10, payout);
        }

        [Fact]
        public void CalculateBetPayout_Field_Multiplier_ReturnsCorrectPayout()
        {
            int mult = 2;

            Bet bet = new Bet {
                Amount = 10,
                BetStatusId = BetStatus.Won,
                GameEventId = GameEvent.Field,
                Id = 1,
                Payout = 0,
                CreateDate = System.DateTime.Now
            };

            int payout = _betService.CalculateBetPayout(bet, mult);

            Assert.Equal(20, payout);
        }
    }
}