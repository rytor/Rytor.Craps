import { useState } from "react";
import Dice from "./Dice";

export default function DiceRoller() {
  const [rolling, setRolling] = useState(false);
  const [roll1, setRoll1] = useState(1);
  const [roll2, setRoll2] = useState(1);

  function handleRoll() {
    const roll1 = Math.floor(Math.random() * 5) + 1;
    const roll2 = Math.floor(Math.random() * 5) + 1;
    console.log("roll will be", roll1, roll2);
    setRolling(true);
    setRoll1(roll1);
    setRoll2(roll2);
    setTimeout(() => {
      setRolling(false);
    }, 5000);
  }

  return (
    <div className="dice-roller">
      <button onClick={() => handleRoll()}>Roll!</button>
      {rolling && (
        <>
          <p>
            Roll: {roll1}, {roll2}{" "}
          </p>
          <div className="dice-roller-wrapper">
            <Dice result={roll1} />
          </div>
          <div className="dice-roller-wrapper">
            <Dice result={roll2} />
          </div>
        </>
      )}
    </div>
  );
}
