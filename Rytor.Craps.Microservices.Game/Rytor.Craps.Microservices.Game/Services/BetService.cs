using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Rytor.Craps.Microservices.Game.Models;
using Rytor.Craps.Microservices.Game.Repositories;
using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Game.Services
{
    public class BetService : IBetService
    {
        private readonly ILogger<BetService> _logger;
        private readonly string _className = "BetService";

        private IEnumerable<GameEventPayout> _gameEventPayouts;
        private readonly IGameRepository _gameRepository;

        public BetService(ILoggerFactory loggerFactory, IGameRepository gameRepository)
        {
             _logger = loggerFactory.CreateLogger<BetService>();
            _logger.LogInformation("BetService initialized.");
            _gameRepository = gameRepository;
            _gameEventPayouts = gameRepository.GetGameEventPayouts();
        }

        public int CalculateBetPayout(Bet bet)
        {
            try
            {
                GameEventPayout payout = _gameEventPayouts.Where(x => x.GameEventId == bet.GameEventId).First();
                return ((bet.Amount * payout.PayoutOddsLeft) / payout.PayoutOddsRight);
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error calculating bet payout for Bet {bet.Id} - {e.Message}");
                return 0;
            }
            
        }

        public bool ValidateBet(Models.Game game, Bet bet)
        {
            try
            {
                // check first if game state is in betting phase
                if (game.State == GameState.OpeningRoll || game.State == GameState.SubsequentRoll)
                    return false;

                // get list of open bets to work against
                IEnumerable<Bet> openBets = _gameRepository.GetBetsByStatus(BetStatus.Open);

                // Validation for creating bet
                if (bet.BetStatusId == BetStatus.Open)
                {
                    // check if open bet already exists      
                    IEnumerable<Bet> openBetsForAccount = openBets.Where(x => x.AccountId == bet.AccountId && x.GameEventId == bet.GameEventId);
                    if (openBetsForAccount.Count() > 0)
                        return false;
                    
                    // if betting on specific numbers or come/don't come, can only do so in subsequent roll bet game state
                    if (bet.GameEventId == GameEvent.Four || bet.GameEventId == GameEvent.Five || bet.GameEventId == GameEvent.Six || bet.GameEventId == GameEvent.Eight || bet.GameEventId == GameEvent.Nine || bet.GameEventId == GameEvent.Ten || bet.GameEventId == GameEvent.Come || bet.GameEventId == GameEvent.DontCome)
                    {
                        if (game.State == GameState.OpeningRollBets)
                            return false;
                    }
                }

                // Validation for cancelling bet
                if (bet.BetStatusId == BetStatus.Cancelled)
                {
                    // check if open bet exists to cancel
                    IEnumerable<Bet> openBetsForAccount = openBets.Where(x => x.AccountId == bet.AccountId && x.GameEventId == bet.GameEventId);
                    if (openBetsForAccount.Count() == 0)
                        return false;
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($@"{_className}: Error validating Bet {bet.Id} - {e.Message}");
                return false;
            }    
        }

        public IEnumerable<Bet> HandleBets(Models.Game game, RollResult roll, IEnumerable<Bet> bets)
        {
            IEnumerable<Bet> openBets = _gameRepository.GetBetsByStatus(BetStatus.Open);

            foreach (Bet bet in bets)
            {
                // handle winning bets
                if (game.LastGameEvents.Any(x => x == bet.GameEventId))
                {
                    bet.BetStatusId = BetStatus.Won;
                    bet.Payout = CalculateBetPayout(bet);
                }
                else
                {
                    // check if last game event constitutes bet loss
                    if (DoesGameEventLoseBet(game, roll, bet))
                    {
                        bet.BetStatusId = BetStatus.Lost;
                        bet.Payout = bet.Amount * -1;
                    }
                }
            }

            return bets;
        }

        private bool DoesGameEventLoseBet(Models.Game game, RollResult roll, Bet bet)
        {
            bool result = false;

            switch (bet.GameEventId) {
                case GameEvent.Pass:
                    if (game.LastGameEvents.Any(x => x == GameEvent.DontPass))
                        return true;
                    break;
                case GameEvent.DontPass:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Pass) && roll.Total != 12)
                        return true;
                    break;
                case GameEvent.Field:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Five || x == GameEvent.Six || x == GameEvent.Seven || x == GameEvent.Eight))
                        return true;
                    break;
                case GameEvent.BigSix:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Seven))
                        return true;
                    break;
                case GameEvent.BigEight:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Seven))
                        return true;
                    break;
                case GameEvent.Come:
                    if (game.LastGameEvents.Any(x => x == GameEvent.DontCome))
                        return true;
                    break;
                case GameEvent.DontCome:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Come))
                        return true;
                    break;
                case GameEvent.Four:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Seven) && game.LastGameEvents.Any(x => x == GameEvent.DontPass))
                        return true;
                    break;
                case GameEvent.Five:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Seven) && game.LastGameEvents.Any(x => x == GameEvent.DontPass))
                        return true;
                    break;
                case GameEvent.Six:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Seven) && game.LastGameEvents.Any(x => x == GameEvent.DontPass))
                        return true;
                    break;
                case GameEvent.Eight:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Seven) && game.LastGameEvents.Any(x => x == GameEvent.DontPass))
                        return true;
                    break;
                case GameEvent.Nine:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Seven) && game.LastGameEvents.Any(x => x == GameEvent.DontPass))
                        return true;
                    break;
                case GameEvent.Ten:
                    if (game.LastGameEvents.Any(x => x == GameEvent.Seven) && game.LastGameEvents.Any(x => x == GameEvent.DontPass))
                        return true;
                    break;
                case GameEvent.C:
                    if (!game.LastGameEvents.Any(x => x == GameEvent.C))
                        return true;
                    break;
                case GameEvent.E:
                    if (!game.LastGameEvents.Any(x => x == GameEvent.E))
                        return true;
                    break;
                case GameEvent.Seven:
                    if (!game.LastGameEvents.Any(x => x == GameEvent.Seven))
                        return true;
                    break;
                case GameEvent.Craps:
                    if (!game.LastGameEvents.Any(x => x == GameEvent.Craps))
                        return true;
                    break;
                case GameEvent.HardFour:
                    if (roll.Total == 4 && !game.LastGameEvents.Any(x => x == GameEvent.HardFour))
                        return true;
                    break;
                case GameEvent.HardSix:
                    if (roll.Total == 6 && !game.LastGameEvents.Any(x => x == GameEvent.HardSix))
                        return true;
                    break;
                case GameEvent.HardEight:
                    if (roll.Total == 8 && !game.LastGameEvents.Any(x => x == GameEvent.HardEight))
                        return true;
                    break;
                case GameEvent.HardTen:
                    if (roll.Total == 10 && !game.LastGameEvents.Any(x => x == GameEvent.HardTen))
                        return true;
                    break;
                case GameEvent.Three:
                    if (!game.LastGameEvents.Any(x => x == GameEvent.Three))
                        return true;
                    break;
                case GameEvent.Two:
                    if (!game.LastGameEvents.Any(x => x == GameEvent.Two))
                        return true;
                    break;
                case GameEvent.Twelve:
                    if (!game.LastGameEvents.Any(x => x == GameEvent.Twelve))
                        return true;
                    break;
                case GameEvent.Eleven:
                    if (!game.LastGameEvents.Any(x => x == GameEvent.Eleven))
                        return true;
                    break;
            }

            return result;
        }
    }
}