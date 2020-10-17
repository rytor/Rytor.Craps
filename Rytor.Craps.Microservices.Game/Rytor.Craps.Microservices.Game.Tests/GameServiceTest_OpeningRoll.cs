using System;
using Xunit;
using Rytor.Craps.Microservices.Game.Services;
using Rytor.Libraries.Dice;
using Rytor.Craps.Microservices.Game.Models;
using System.Collections.Generic;

namespace Rytor.Craps.Microservices.Game.Tests
{
    public class GameServiceTest_OpeningRoll
    {
        private readonly GameService _gameService;

        public GameServiceTest_OpeningRoll()
        {
            _gameService = new GameService();

            // Override Game State to OpeningRoll for tests
            Models.Game newGame = new Models.Game();
            newGame.State = GameState.OpeningRoll;
            _gameService.OverrideGame(newGame);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_TwoRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(1, 1);

            // call HandleRoll with a two
            _game = _gameService.HandleRoll(roll);

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
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(1, 2);

            // call HandleRoll with a three
            _game = _gameService.HandleRoll(roll);

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
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(3, 1);

            // call HandleRoll with a four
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 4);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Four); // Four should only be triggered on subsequent roll
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardFour);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_HardFourRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(2, 2);

            // call HandleRoll with a four
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 4);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardFour);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Four); // Four should only be triggered on subsequent roll
        }

        [Fact]
        public void HandleRoll_OpeningRoll_FiveRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(2, 3);

            // call HandleRoll with a five
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 5);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Five); // Five should only be triggered on subsequent roll
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_SoftSixRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(5, 1);

            // call HandleRoll with a six
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 6);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Six); // Six should only be triggered on subsequent roll
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardSix);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_HardSixRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(3, 3);

            // call HandleRoll with a six
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 6);           
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardSix);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Six); // Six should only be triggered on subsequent roll
        }

        [Fact]
        public void HandleRoll_OpeningRoll_SevenRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(3, 4);

            // call HandleRoll with a seven
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Seven);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_SoftEightRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(6, 2);

            // call HandleRoll with a eight
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 8);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Eight); // Eight should only be triggered on subsequent roll
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardEight);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_HardEightRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(4, 4);

            // call HandleRoll with a eight
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 8);           
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardEight);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Eight); // Eight should only be triggered on subsequent roll
        }

        [Fact]
        public void HandleRoll_OpeningRoll_NineRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(4, 5);

            // call HandleRoll with a nine
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 9);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Nine); // Nine should only be triggered on subsequent roll          
        }

        [Fact]
        public void HandleRoll_OpeningRoll_SoftTenRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(6, 4);

            // call HandleRoll with a ten
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 10);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Ten); // Ten should only be triggered on subsequent roll
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardTen);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_HardTenRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(5, 5);

            // call HandleRoll with a ten
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.True(_game.Point == 10);           
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardTen);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Eight); // Ten should only be triggered on subsequent roll
        }

        [Fact]
        public void HandleRoll_OpeningRoll_ElevenRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(6, 5);

            // call HandleRoll with a eleven
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Eleven);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.E);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_OpeningRoll_TwelveRolled_ReturnTrue()
        {
            // instantiate Game
            Models.Game _game;
            
            // instantiate roll result
            RollResult roll = ForceRollResult(6, 6);

            // call HandleRoll with a twelve
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.DontPass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Twelve);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Craps);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.C);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
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
