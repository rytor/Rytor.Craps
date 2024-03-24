import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { store } from "../../app/store";
import { GameStateMachine } from "./GameStateMachine";

export enum BetLocation {
  Place4 = "Place4",
  Place9 = "Place9",
  Hard6 = "Hard6",
  Big6 = "Big6",
}

export interface Bet {
  player: string;
  amount: number;
  location: string;
}

export enum EBetState {
  OPEN = "OPEN",
  CANCELLED = "CANCELLED",
  PUSH = "PUSH",
  WON = "WON",
  LOST = "LOST",
}

export enum EPointSlots {
  None = "None",
  Place4 = "Place4",
  Place5 = "Place5",
  Place6 = "Place6",
  Place8 = "Place8",
  Place9 = "Place9",
  Place10 = "Place10",
}

export enum EGamePhase {
  OpeningRollBets = "OpeningRollBets", // display "Bets are open"
  OpeningRoll = "OpeningRoll", // show the dice roll, start a 3 second timeout and then render winners
  SubsequentRollBets = "SubsequentRollBets",
  SubsequentRoll = "SubsequentRoll",
}

export enum EGameEvent {
  PASS = "PASS",
  SEVEN = "SEVEN",
  FIELD = "FIELD",
}

export interface IDice {
  value: number;
  numberOfSides: number;
}

export interface IDiceRoll {
  values: IDice[];
  total: number;
}

export interface IGameState {
  phase: string;
  numberOfRolls: number;
  point: string;
  timeLeft: number;
  dice: IDiceRoll;
  bets: Bet[];
  lastGameEvents: EGameEvent[];
  isComplete: boolean;
  showDice?: boolean;
}

const initialState: IGameState = {
  phase: "OpeningRollBets",
  numberOfRolls: 0,
  point: "four",
  timeLeft: 30,
  dice: {
    values: [
      { value: 1, numberOfSides: 6 },
      { value: 1, numberOfSides: 6 },
    ],
    total: 2,
  },
  bets: [],
  lastGameEvents: [],
  isComplete: false,

  // output
  showDice: false,
};

const gameSlice = createSlice({
  name: "game",
  initialState,
  reducers: {
    mergeState(state: IGameState, action: PayloadAction<any>) {
      const prevPhase = state.phase;
      const currentPhase = action.payload.phase;

      state.phase = action.payload.phase;
      state.numberOfRolls = action.payload.numberOfRolls;
      state.point = action.payload.point;
      state.bets = [...action.payload.bets];
      state.lastGameEvents = action.payload.lastGameEvents;
      state.isComplete = action.payload.isComplete;
      state.dice = { ...action.payload.dice };

      if (
        prevPhase === EGamePhase.OpeningRollBets &&
        currentPhase === EGamePhase.OpeningRoll
      ) {
        console.log("show!");
        state.showDice = true;
      }
      // stateMachine.transition(prevPhase, currentPhase);
    },
    rollStart(state: IGameState, action: PayloadAction<IDiceRoll>) {
      state.dice = action.payload;
      state.showDice = true;
    },
    rollEnd(state: IGameState) {
      state.showDice = false;
    },
  },
});

export const { mergeState, rollStart, rollEnd } = gameSlice.actions;
export default gameSlice.reducer;
