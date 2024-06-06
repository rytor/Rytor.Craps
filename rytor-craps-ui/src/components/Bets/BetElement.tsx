import { useAppSelector } from "../../app/hooks";
import { Bet } from "../../features/game/game-slice";
import Chip from "../Chip/Chip";
import Point from "../Point/Point";

export function BetElement({bet} : {bet: string}) {
  const game = useAppSelector((state) => state.game);

  const filteredBets = game.bets.filter((b: Bet) => b.location === bet);

  return (
    <>
      <div className='absolute' id={`bet-${bet}`}>
        {!!filteredBets.length && <Chip total={filteredBets.length} />}
        { game.point===bet && <Point />}
      </div>
    </>
  )
}