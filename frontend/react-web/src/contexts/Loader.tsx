import {useLoader} from "./LoaderContext.tsx";
import {Spinner} from "react-bootstrap";
import "./index.scss";

export const Loader = () => {
  const {loading} = useLoader();

  if (!loading) return null;

  return (
    <div className={"custom-loader"}>
      <Spinner variant={"primary"}/>
    </div>
  );
};
