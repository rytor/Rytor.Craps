// adapted from https://codepen.io/maxew33/pen/bGaWYEw
import { useEffect, useState } from "react";
import "./DiceRoll.scss";
import { diceValueLookupTable } from "./diceValueLookup";

// note: theres some weirdness to how the CSS transform sorta "stays" around.
//  If this component is not removed from the dom tree it can act up.

export default function Dice({ result }: { result: number }) {
  const [angleX, setAngleX] = useState(0);
  const [angleY, setAngleY] = useState(0);
  const [delay, setDelay] = useState(1);

  function roll(result: number) {
    // scan the lookup table for all rolls that would result in the value supplied as "result"
    const rolls = diceValueLookupTable.filter((roll) => roll.result === result);

    if (!rolls.length) {
      console.log("error looking up roll", result);
      return;
    }

    // select a random roll from the known rolls
    const selectedRoll = rolls[~~(Math.random() * rolls.length)];

    // extend the animation based on the value
    setDelay(Math.max(selectedRoll.x, selectedRoll.y) * 250);

    // rotate the dice to match the desired result
    setAngleX(angleX + 90 * selectedRoll.x);
    setAngleY(angleY + 90 * selectedRoll.y);

    return result;
  }

  useEffect(() => {
    roll(result);
  }, [result]);

  return (
    <div className="dice-wrapper">
      <div className="dice-container">
        <div
          className="dice"
          style={{
            transform: `rotateX(${angleX}deg) rotateY(${angleY}deg)`,
            transitionDuration: `${delay}ms`,
          }}
        >
          <div className="face" data-id="1">
            <div className="point point-middle point-center"></div>
          </div>
          <div className="face" data-id="2">
            <div className="point point-top point-right"></div>
            <div className="point point-bottom point-left"></div>
          </div>
          <div className="face" data-id="6">
            <div className="point point-top point-right"></div>
            <div className="point point-top point-left"></div>
            <div className="point point-middle point-right"></div>
            <div className="point point-middle point-left"></div>
            <div className="point point-bottom point-right"></div>
            <div className="point point-bottom point-left"></div>
          </div>
          <div className="face" data-id="5">
            <div className="point point-top point-right"></div>
            <div className="point point-top point-left"></div>
            <div className="point point-middle point-center"></div>
            <div className="point point-bottom point-right"></div>
            <div className="point point-bottom point-left"></div>
          </div>
          <div className="face" data-id="3">
            <div className="point point-top point-right"></div>
            <div className="point point-middle point-center"></div>
            <div className="point point-bottom point-left"></div>
          </div>
          <div className="face" data-id="4">
            <div className="point point-top point-right"></div>
            <div className="point point-top point-left"></div>
            <div className="point point-bottom point-right"></div>
            <div className="point point-bottom point-left"></div>
          </div>
        </div>
      </div>
    </div>
  );
}
