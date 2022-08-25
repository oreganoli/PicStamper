import * as React from "react";
import * as ReactDom from "react-dom/client";
import { JobList } from "./components/JobList";
let root = ReactDom.createRoot(document.getElementById("root"));
root.render(<JobList />);