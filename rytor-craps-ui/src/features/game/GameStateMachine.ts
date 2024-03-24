// import { store } from "../../app/store";
// import { EGamePhase, rollEnd, rollStart } from "./game-slice";

export class GameStateMachine {
  constructor(private store: any) {}
  public transition(from: string, to: string) {
    // if (from === EGamePhase.OpeningRollBets && to === EGamePhase.OpeningRoll) {
    //   this.store.dispatch(rollStart());
    //   setTimeout(() => {
    //     this.store.dispatch(rollEnd());
    //   }, 5000);
    // }
  }
}
