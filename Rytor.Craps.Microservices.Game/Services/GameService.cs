using Rytor.Craps.Microservices.Game.Models;
using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Game.Services
{
    public class GameService : IGameService
    {
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
                        game.State = GameState.SubsequentRoll;
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

                    if (roll.Total == 6)
                    {
                        game.LastGameEvents.Add(GameEvent.BigSix);
                    }
                    else if (roll.Total == 8)
                    {
                        game.LastGameEvents.Add(GameEvent.BigEight);
                    }
                    break;
                default:
                    break;
            }

            //Single Line Bets

            //Moving into different variables solely for easier typing
            int d1 = roll.Dice[0].Value;
            int d2 = roll.Dice[1].Value;

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
                game.LastGameEvents.Add(GameEvent.Four);
            }
            else if (d1 == 3 && d2 == 3)
            {
                game.LastGameEvents.Add(GameEvent.Six);
            }
            else if (d1 == 4 && d2 == 4)
            {
                game.LastGameEvents.Add(GameEvent.Eight);
            }
            else if (d1 == 5 && d2 == 5)
            {
                game.LastGameEvents.Add(GameEvent.Ten);
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

            if (IsRollInField(roll.Total))
            {
                game.LastGameEvents.Add(GameEvent.Field);
            }
            if (IsRollCraps(roll.Total))
            {
                game.LastGameEvents.Add(GameEvent.C);
            }
            if (roll.Total == 7)
            {
                game.LastGameEvents.Add(GameEvent.Seven);
            }

            return game;
        }

        public Models.Game ResetGame()
        {
            return new Models.Game();
        }

        private bool CanRollBePoint(int rollValue)
        {
            if (rollValue == 4 || rollValue == 5 || rollValue == 6 || rollValue == 8 || rollValue == 9 || rollValue == 10)
                return true;
            else
                return false;
        }

        private bool IsRollInField(int rollValue)
        {
            if (rollValue == 2 || rollValue == 3 || rollValue == 4 ||  rollValue == 9 || rollValue == 10 || rollValue == 11 || rollValue == 12)
                return true;
            else
                return false;
        }

        private bool IsRollCraps(int rollValue)
        {
            if (rollValue == 2 || rollValue == 3 || rollValue == 12)
                return true;
            else
                return false;
        }

        private bool IsRollOpeningPassWin(int rollValue)
        {
            if (rollValue == 7 || rollValue == 11)
                return true;
            else
                return false;
        }
    }
}