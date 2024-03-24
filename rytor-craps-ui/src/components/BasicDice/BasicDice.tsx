import { useEffect } from "react";
import { useAppDispatch } from "../../app/hooks";
import { rollEnd } from "../../features/game/game-slice";

export function BasicDice({ targets }: { targets: number[] }) {
  const dispatch = useAppDispatch();

  useEffect(() => {
    setTimeout(() => {
      dispatch(rollEnd());
    }, 5000);
  }, []);
  return (
    <div className="absolute top-0 left-1/2">
      <h1 className="text-2xl">
        Dice values: {targets[0]}, {targets[1]}
      </h1>
    </div>
  );
}
