import { configureStore } from "@reduxjs/toolkit";
import gameReducer from "../features/game/game-slice";
import uiReducer from "../features/ui/ui-slice";

export const store = configureStore({
  reducer: {
    game: gameReducer,
    ui: uiReducer,
  },
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
