import axios from "axios";
import { useEffect, useState } from "react";
import "./App.scss";
import { useAppDispatch, useAppSelector } from "./app/hooks";
import ActivePlayers from "./components/ActivePlayers/ActivePlayers";
import { BasicDice } from "./components/BasicDice/BasicDice";
import Debug from "./components/Debug/Debug";
import DiceRoller from "./components/DiceRoll/DiceRoller";
import ThreeJsDice from "./components/DiceRoll/ThreeJsDice";
import Table from "./components/Table/Table";
import gameSlice, { mergeState } from "./features/game/game-slice";

let rendered = false;

export default function App() {
  const dispatch = useAppDispatch();
  const ui = useAppSelector((state) => state.ui);
  const game = useAppSelector((state) => state.game);

  useEffect(() => {
    // if we have already started the loop, bail out out
    if (rendered) return;
    rendered = true;
    getLatestState();

    // set a timer to get the latest state
    setInterval(async () => {
      getLatestState();
    }, 5000);
  }, [game.showDice]);

  const getLatestState = async () => {
    if (ui.useDebugState) return;
    let state = await axios.get(`http://localhost:3001/state`);
    dispatch(mergeState(state.data));
  };

  return (
    <div id="app">
      <Debug />
      <Table />
      <ActivePlayers />
      {/* <DiceRoller /> */}
      {game.showDice ? "SHOW" : "HIDE"}
      {game.showDice && (
        <BasicDice targets={game.dice.values.map((d) => d.value)} />
      )}
      {game.showDice && (
        <ThreeJsDice targets={game.dice.values.map((d) => d.value)} />
      )}
    </div>
  );
}
