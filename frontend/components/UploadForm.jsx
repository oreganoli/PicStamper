import * as React from "react";
import { useState } from "react";
export default function UploadForm({ jobId, uploadUrl }) {
    let [canStart, setCanStart] = useState(true);
    let [jobState, setJobState] = useState("pending");
    let stateDescriptions = {
        "pending": "Start job",
        "inProgress": "Processing..."
    };
    return <>
        <h1>Job #{jobId}</h1>
        <button disabled={!canStart} onClick={() => {
            setCanStart(false);
            setJobState("inProgress");
        }}>{stateDescriptions[jobState]}</button>
    </>
}