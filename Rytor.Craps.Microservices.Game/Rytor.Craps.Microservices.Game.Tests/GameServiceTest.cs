using System;
using Xunit;
using Rytor.Craps.Microservices.Game.Services;
using Rytor.Libraries.Dice;
using Rytor.Craps.Microservices.Game.Models;
using System.Collections.Generic;

namespace Rytor.Craps.Microservices.Game.Tests
{
    public class GameServiceTest
    {
        private Models.Game _game;
        private readonly GameService _gameService;

        public GameServiceTest()
        {
            _gameService = new GameService();
        }

        [Fact]
        public void HandleRoll_OpeningRoll_TwoRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            
            // instantiate roll result
            RollResult roll = ForceRollResult(1, 1);

            // call HandleRoll with a two
            _game = _gameService.HandleRoll(roll, _game);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.DontPass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Two);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Craps);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.C);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_ThreeRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            
            // instantiate roll result
            RollResult roll = ForceRollResult(1, 2);

            // call HandleRoll with a three
            _game = _gameService.HandleRoll(roll, _game);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.DontPass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Three);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Craps);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.C);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_SoftFourRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            
            // instantiate roll result
            RollResult roll = ForceRollResult(3, 1);

            // call HandleRoll with a four
            _game = _gameService.HandleRoll(roll, _game);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 4);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Four);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_HardFourRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            
            // instantiate roll result
            RollResult roll = ForceRollResult(2, 2);

            // call HandleRoll with a four
            _game = _gameService.HandleRoll(roll, _game);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 4);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Four); // Four should only be triggered on subsequent roll
        }

        [Fact]
        public void HandleRoll_OpeningRoll_FiveRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            
            // instantiate roll result
            RollResult roll = ForceRollResult(2, 3);

            // call HandleRoll with a five
            _game = _gameService.HandleRoll(roll, _game);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 5);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Five);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_SevenRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            
            // instantiate roll result
            RollResult roll = ForceRollResult(3, 4);

            // call HandleRoll with a seven
            _game = _gameService.HandleRoll(roll, _game);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Seven);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_ElevenRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            
            // instantiate roll result
            RollResult roll = ForceRollResult(6, 5);

            // call HandleRoll with a eleven
            _game = _gameService.HandleRoll(roll, _game);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Eleven);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.E);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        private RollResult ForceRollResult(int dice1Value, int dice2Value)
        {
            RollResult rollResult = new RollResult();
            rollResult.Dice = new List<Die>();
            rollResult.Dice.Add(new Die(6));
            rollResult.Dice.Add(new Die(6));
            rollResult.Dice[0].Value = dice1Value;
            rollResult.Dice[1].Value = dice2Value;
            rollResult.Total = dice1Value + dice2Value;
            return rollResult;
        }
    }
}
