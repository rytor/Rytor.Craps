using System;
using Xunit;
using Rytor.Craps.Microservices.Game.Services;
using Rytor.Libraries.Dice;
using Rytor.Craps.Microservices.Game.Models;
using System.Collections.Generic;

namespace Rytor.Craps.Microservices.Game.Tests
{
    public class GameServiceTest_SubsequentRoll
    {
        private Models.Game _game;
        private readonly GameService _gameService;

        public GameServiceTest_SubsequentRoll()
        {
            _gameService = new GameService();
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_TwoRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 6;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(1, 1);

            // call HandleRoll with a two
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Two);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Craps);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.C);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_ThreeRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 6;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(2, 1);

            // call HandleRoll with a three
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Three);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Craps);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.C);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_SoftFourRolled_IsPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 4;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(1, 3);

            // call HandleRoll with a four
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Four); // Can't place bet on Four if point is already Four
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardFour);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_HardFourRolled_IsPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 4;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(2, 2);

            // call HandleRoll with a four
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardFour);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Four); // Can't place bet on Four if point is already Four
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_SoftFourRolled_IsNotPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 5;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(1, 3);

            // call HandleRoll with a four
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Four);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardFour);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_HardFourRolled_IsNotPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 5;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(2, 2);

            // call HandleRoll with a four
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);            
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardFour);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Four);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);          
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_FiveRolled_IsPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 5;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(2, 3);

            // call HandleRoll with a five
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Five); // Can't place bet on Five if point is already Five
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_FiveRolled_IsNotPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 8;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(2, 3);

            // call HandleRoll with a five
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Five);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);            
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_SoftSixRolled_IsPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 6;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(2, 4);

            // call HandleRoll with a six
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Six); // Can't place bet on six if point is already six
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardSix);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_HardSixRolled_IsPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 6;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(3, 3);

            // call HandleRoll with a six
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardSix);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);           
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Six); // Can't place bet on six if point is already six
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_SoftSixRolled_IsNotPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 4;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(5, 1);

            // call HandleRoll with a six
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Six);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardSix);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_HardSixRolled_IsNotPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 10;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(3, 3);

            // call HandleRoll with a six
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);            
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardSix);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Six);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);          
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_SevenRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 5;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(4, 3);

            // call HandleRoll with a seven
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.DontPass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Seven);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_SoftEightRolled_IsPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 8;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(6, 2);

            // call HandleRoll with a eight
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Eight); // Can't place bet on eight if point is already eight
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardEight);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_HardEightRolled_IsPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 8;
            
            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(4, 4);

            // call HandleRoll with a eight
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardEight);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);           
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Eight); // Can't place bet on eight if point is already eight
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_SoftEightRolled_IsNotPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 9;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(3, 5);

            // call HandleRoll with a eight
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Eight);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardEight);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_HardEightRolled_IsNotPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 4;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(4, 4);

            // call HandleRoll with a eight
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);            
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardEight);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Eight);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);          
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_NineRolled_IsPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 9;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(4, 5);

            // call HandleRoll with a nine
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Nine); // Can't place bet on nine if point is already nine
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_NineRolled_IsNotPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 8;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(5, 4);

            // call HandleRoll with a nine
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Nine);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);            
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_SoftTenRolled_IsPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 10;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(4, 6);

            // call HandleRoll with a ten
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Ten); // Can't place bet on ten if point is already ten
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardTen);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_HardTenRolled_IsPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 10;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(5, 5);

            // call HandleRoll with a ten
            _game = _gameService.HandleRoll(roll);

            // Make sure game is marked as completed and correct events are triggered
            Assert.True(_game.Completed == true);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardTen);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);           
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Ten); // Can't place bet on ten if point is already ten
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_SoftTenRolled_IsNotPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 9;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(6, 4);

            // call HandleRoll with a ten
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Ten);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.HardTen);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_HardTenRolled_IsNotPoint_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 6;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(5, 5);

            // call HandleRoll with a ten
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);            
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.HardTen);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Ten);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);          
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_ElevenRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 10;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(5, 6);

            // call HandleRoll with a eleven
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Eleven);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.E);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.DontPass);
        }

        [Fact]
        public void HandleRoll_SubsequentRoll_TwelveRolled_ReturnTrue()
        {
            // instantiate Game
            _game = new Models.Game();
            _game.State = GameState.SubsequentRoll;
            _game.Point = 9;

            // override Game
            _game = _gameService.OverrideGame(_game);
            
            // instantiate roll result
            RollResult roll = ForceRollResult(6, 6);

            // call HandleRoll with a twelve
            _game = _gameService.HandleRoll(roll);

            // Make sure game isn't marked as completed and correct events are triggered
            Assert.True(_game.Completed == false);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Twelve);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Craps);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.C);
            Assert.Contains(_game.LastGameEvents, x => x == GameEvent.Field);
            Assert.DoesNotContain(_game.LastGameEvents, x => x == GameEvent.Pass);
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
