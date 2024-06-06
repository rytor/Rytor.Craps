import { BetElement } from "../Bets/BetElement";

export const betLocations: string[] = [
  "any",
  "big6",
  "big8",
  "cecraps",
  "ceeleven",
  "come",
  "dontcome",
  "dontpass",
  "eleven",
  "field",
  "hard4",
  "hard6",
  "hard8",
  "hard10",
  "pass",
  "four",
  "five",
  "six",
  "seven",
  "eight",
  "nine",
  "ten",
  "three",
  "twelve",
  "two",
];

export default function Board() {
  return (
    <div id="board">
      {betLocations.map((location: string, i: number) => (
        <BetElement key={i} bet={location} />
      ))}
    </div>
  );
}
