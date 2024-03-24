import { useAppSelector } from "../../app/hooks";
import { Bet } from "../../features/game/game-slice";

export default function ActivePlayers() {
  const game = useAppSelector((state) => state.game);

  return (
    <div id="active-players">
      <table>
        <tbody>
          <tr>
            <th>Player</th>
            <th>Bet</th>
          </tr>
          {game.bets.map((bet: Bet, i: number) => (
            <tr key={i}>
              <td>{bet.player}</td>
              <td>{bet.amount} on {bet.location}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
