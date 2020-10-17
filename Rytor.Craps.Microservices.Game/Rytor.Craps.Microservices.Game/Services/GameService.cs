using Rytor.Craps.Microservices.Game.Models;
using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Game.Services
{
    // Service which handles _game progress and reporting of specific game events.
    public class GameService : IGameService
    {
        private Models.Game _game;

        public GameService()
        {
            _game = ResetGame();
        }

        // Function to return current game
        public Models.Game GetGame()
        {
            return _game;
        }

        // Primary function which occurs after a roll has taken place - updates any events triggered by game state and specific roll
        public Models.Game HandleRoll(RollResult roll)
        {
            _game.NumberOfRolls++;
            _game.LastGameEvents.Clear();

            // Line Bets
            switch (_game.State)
            {
                case GameState.OpeningRoll:
                    if (CanRollBePoint(roll.Total))
                    {
                        _game.Point = roll.Total;
                    }
                    else if (IsRollOpeningPassWin(roll.Total))
                    {
                        _game.LastGameEvents.Add(GameEvent.Pass);
                        _game.Completed = true;
                    }
                    else if (IsRollCraps(roll.Total))
                    {
                        _game.LastGameEvents.Add(GameEvent.DontPass);
                        _game.Completed = true;
                    }
                    break;
                case GameState.SubsequentRoll:
                    // Pass line
                    if (roll.Total == _game.Point)
                    {
                        _game.LastGameEvents.Add(GameEvent.Pass);
                        _game.Completed = true;
                    }
                    else if (roll.Total == 7)
                    {
                        _game.LastGameEvents.Add(GameEvent.DontPass);
                        _game.Completed = true;
                    }

                    // Other non-point rolls
                    if (roll.Total == 4 && 4 != _game.Point)
                    {
                        _game.LastGameEvents.Add(GameEvent.Four);
                    }
                    else if (roll.Total == 5 && 5 != _game.Point)
                    {
                        _game.LastGameEvents.Add(GameEvent.Five);
                    }
                    else if (roll.Total == 6 && 6 != _game.Point)
                    {
                        _game.LastGameEvents.Add(GameEvent.Six);
                    }
                    else if (roll.Total == 8 && 8 != _game.Point)
                    {
                        _game.LastGameEvents.Add(GameEvent.Eight);
                    }
                    else if (roll.Total == 9 && 9 != _game.Point)
                    {
                        _game.LastGameEvents.Add(GameEvent.Nine);
                    }
                    else if (roll.Total == 10 && 10 != _game.Point)
                    {
                        _game.LastGameEvents.Add(GameEvent.Ten);
                    }

                    // Big six and eight
                    if (roll.Total == 6)
                    {
                        _game.LastGameEvents.Add(GameEvent.BigSix);
                    }
                    if (roll.Total == 8)
                    {
                        _game.LastGameEvents.Add(GameEvent.BigEight);
                    }

                    break;
                default:
                    break;
            }

            // Single Line Bets

            // Moving into different variables solely for easier typing
            int d1 = roll.Dice[0].Value;
            int d2 = roll.Dice[1].Value;

            // Hardway bets
            if (d1 == 1 && d2 == 1)
            {
                _game.LastGameEvents.Add(GameEvent.Two);
            }
            else if ((d1 == 1 && d2 == 2) || (d1 == 2 && d2 == 1))
            {
                _game.LastGameEvents.Add(GameEvent.Three);
            }
            else if (d1 == 2 && d2 == 2)
            {
                _game.LastGameEvents.Add(GameEvent.HardFour);
            }
            else if (d1 == 3 && d2 == 3)
            {
                _game.LastGameEvents.Add(GameEvent.HardSix);
            }
            else if (d1 == 4 && d2 == 4)
            {
                _game.LastGameEvents.Add(GameEvent.HardEight);
            }
            else if (d1 == 5 && d2 == 5)
            {
                _game.LastGameEvents.Add(GameEvent.HardTen);
            }
            else if ((d1 == 5 && d2 == 6) || (d1 == 6 && d2 == 5))
            {
                _game.LastGameEvents.Add(GameEvent.Eleven);
                _game.LastGameEvents.Add(GameEvent.E);
            }
            else if (d1 == 6 && d2 == 6)
            {
                _game.LastGameEvents.Add(GameEvent.Twelve);
            }

            // Field bet
            if (IsRollInField(roll.Total))
            {
                _game.LastGameEvents.Add(GameEvent.Field);
            }
            // Crap Out bet
            if (IsRollCraps(roll.Total))
            {
                _game.LastGameEvents.Add(GameEvent.Craps);
                _game.LastGameEvents.Add(GameEvent.C);
            }
            // Single-Roll Seven bet
            if (roll.Total == 7)
            {
                _game.LastGameEvents.Add(GameEvent.Seven);
            }

            return _game;
        }

        // Function to set _game object to clean slate following _game completion
        public Models.Game ResetGame()
        {
            _game = new Models.Game();
            return _game;
        }

        // Function to advance _game progress, depending on situation
        public Models.Game AdvanceGameState()
        {
            // If _game completed (ie. opening roll hits 2/3/7/11/12, point is hit, seven is hit after opening roll) start all over
            if (_game.Completed)
            {
                _game = ResetGame();
            }
            else
            {
                switch (_game.State)
                {
                    // After bet period, start with opening roll
                    case GameState.OpeningRollBets:
                        _game.State = GameState.OpeningRoll;
                        break;
                    // After opening roll completed, start subsequent roll bet period
                    case GameState.OpeningRoll:
                        _game.State = GameState.SubsequentRollBets;
                        break;
                    // After bet period, start subsequent roll
                    case GameState.SubsequentRollBets:
                        _game.State = GameState.SubsequentRoll;
                        break;
                    // If on subsequent roll, and _game not completed, stay there until _game completion
                    default:
                        break;
                }
            }

            return _game;
        }

        // Function to force game into specific state. Mainly used to set up unit tests.
        public Models.Game OverrideGame(Models.Game newGame)
        {
            _game = newGame;
            return _game;
        }

        // Helper function to determine if opening roll can move to subsequent roll, because total rolled can be a point
        private bool CanRollBePoint(int rollValue)
        {
            if (rollValue == 4 || rollValue == 5 || rollValue == 6 || rollValue == 8 || rollValue == 9 || rollValue == 10)
                return true;
            else
                return false;
        }

        // Helper function to determine if field bet has been hit on current roll
        private bool IsRollInField(int rollValue)
        {
            if (rollValue == 2 || rollValue == 3 || rollValue == 4 ||  rollValue == 9 || rollValue == 10 || rollValue == 11 || rollValue == 12)
                return true;
            else
                return false;
        }

        // Helper function to determine if craps has been hit on current roll
        private bool IsRollCraps(int rollValue)
        {
            if (rollValue == 2 || rollValue == 3 || rollValue == 12)
                return true;
            else
                return false;
        }

        // Helper function to determine if opening roll has resulted in a pass line win
        private bool IsRollOpeningPassWin(int rollValue)
        {
            if (rollValue == 7 || rollValue == 11)
                return true;
            else
                return false;
        }
    }
}