import * as React from "react";
import { useState } from "react";
export default function Main() {
    let [job, setJob] = useState(null);

    if (job == null) {
        return <button onClick={() => {
            getJob().then(r => setJob(r));
        }}>Start new processing job</button>;
    } else {
        return <p>Job ID: {job.jobId}</p>;
    }
}

async function getJob() {
    let response = await fetch("https://picstamper-api.oreganoli.xyz/newJob", {
        method: "POST"
    }).then(success => success.json(), _failure => {
        return {
            "jobId": "error placeholder" // my AWS account is suspended ATM
            // TODO: remove this for production
        };
    })
    return await response;
}
// function 