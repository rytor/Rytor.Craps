using Rytor.Craps.Microservices.Game.Models;
using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Game.Services
{
    // Service which handles game progress and reporting of specific game events.
    public class GameService : IGameService
    {
        // Primary function which occurs after a roll has taken place - updates any events triggered by game state and specific roll
        public Models.Game HandleRoll(RollResult roll, Models.Game game)
        {
            game.NumberOfRolls++;
            game.LastGameEvents.Clear();

            // Line Bets
            switch (game.State)
            {
                case GameState.OpeningRoll:
                    if (CanRollBePoint(roll.Total))
                    {
                        game.Point = roll.Total;
                    }
                    else if (IsRollOpeningPassWin(roll.Total))
                    {
                        game.LastGameEvents.Add(GameEvent.Pass);
                        game.Completed = true;
                    }
                    else if (IsRollCraps(roll.Total))
                    {
                        game.LastGameEvents.Add(GameEvent.DontPass);
                        game.Completed = true;
                    }
                    break;
                case GameState.SubsequentRoll:
                    // Pass line
                    if (roll.Total == game.Point)
                    {
                        game.LastGameEvents.Add(GameEvent.Pass);
                        game.Completed = true;
                    }
                    else if (roll.Total == 7)
                    {
                        game.LastGameEvents.Add(GameEvent.DontPass);
                        game.Completed = true;
                    }

                    // Other non-point rolls
                    if (roll.Total == 4 && 4 != game.Point)
                    {
                        game.LastGameEvents.Add(GameEvent.Four);
                    }
                    else if (roll.Total == 5 && 5 != game.Point)
                    {
                        game.LastGameEvents.Add(GameEvent.Five);
                    }
                    else if (roll.Total == 6 && 6 != game.Point)
                    {
                        game.LastGameEvents.Add(GameEvent.Six);
                    }
                    else if (roll.Total == 8 && 8 != game.Point)
                    {
                        game.LastGameEvents.Add(GameEvent.Eight);
                    }
                    else if (roll.Total == 9 && 9 != game.Point)
                    {
                        game.LastGameEvents.Add(GameEvent.Nine);
                    }
                    else if (roll.Total == 10 && 10 != game.Point)
                    {
                        game.LastGameEvents.Add(GameEvent.Ten);
                    }

                    // Big six and eight
                    if (roll.Total == 6)
                    {
                        game.LastGameEvents.Add(GameEvent.BigSix);
                    }
                    if (roll.Total == 8)
                    {
                        game.LastGameEvents.Add(GameEvent.BigEight);
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
                game.LastGameEvents.Add(GameEvent.Two);
            }
            else if ((d1 == 1 && d2 == 2) || (d1 == 2 && d2 == 1))
            {
                game.LastGameEvents.Add(GameEvent.Three);
            }
            else if (d1 == 2 && d2 == 2)
            {
                game.LastGameEvents.Add(GameEvent.HardFour);
            }
            else if (d1 == 3 && d2 == 3)
            {
                game.LastGameEvents.Add(GameEvent.HardSix);
            }
            else if (d1 == 4 && d2 == 4)
            {
                game.LastGameEvents.Add(GameEvent.HardEight);
            }
            else if (d1 == 5 && d2 == 5)
            {
                game.LastGameEvents.Add(GameEvent.HardTen);
            }
            else if ((d1 == 5 && d2 == 6) || (d1 == 6 && d2 == 5))
            {
                game.LastGameEvents.Add(GameEvent.Eleven);
                game.LastGameEvents.Add(GameEvent.E);
            }
            else if (d1 == 6 && d2 == 6)
            {
                game.LastGameEvents.Add(GameEvent.Twelve);
            }

            // Field bet
            if (IsRollInField(roll.Total))
            {
                game.LastGameEvents.Add(GameEvent.Field);
            }
            // Crap Out bet
            if (IsRollCraps(roll.Total))
            {
                game.LastGameEvents.Add(GameEvent.Craps);
                game.LastGameEvents.Add(GameEvent.C);
            }
            // Single-Roll Seven bet
            if (roll.Total == 7)
            {
                game.LastGameEvents.Add(GameEvent.Seven);
            }

            return game;
        }

        // Function to set game object to clean slate following game completion
        public Models.Game ResetGame()
        {
            return new Models.Game();
        }

        // Function to advance game progress, depending on situation
        public Models.Game AdvanceGameState(Models.Game game)
        {
            // If game completed (ie. opening roll hits 2/3/7/11/12, point is hit, seven is hit after opening roll) start all over
            if (game.Completed)
            {
                return ResetGame();
            }
            else
            {
                switch (game.State)
                {
                    // After bet period, start with opening roll
                    case GameState.OpeningRollBets:
                        game.State = GameState.OpeningRoll;
                        break;
                    // After opening roll completed, start subsequent roll bet period
                    case GameState.OpeningRoll:
                        game.State = GameState.SubsequentRollBets;
                        break;
                    // After bet period, start subsequent roll
                    case GameState.SubsequentRollBets:
                        game.State = GameState.SubsequentRoll;
                        break;
                    // If on subsequent roll, and game not completed, stay there until game completion
                    default:
                        break;
                }
            }

            return game;
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