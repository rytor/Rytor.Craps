import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export interface IUIState {
  useDebugState: boolean;
}

const initialState: IUIState = {
  useDebugState: true,
};

const uiSlice = createSlice({
  name: "ui",
  initialState,
  reducers: {
    setUseDebugState(state: IUIState, action: PayloadAction<boolean>) {
      state.useDebugState = action.payload;
    },
  },
});

export const { setUseDebugState } = uiSlice.actions;
export default uiSlice.reducer;
