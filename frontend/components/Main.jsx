import * as React from "react";
import { useState } from "react";
import DownloadScreen from "./DownloadScreen";
import UploadForm from "./UploadForm";
import UploadScreen from "./UploadScreen";
export default function Main() {
    let [job, setJob] = useState(null);
    let [uploadFiles, setUploadFiles] = useState([]);
    let [uploadFinished, setUploadFinished] = useState(false);
    if (job == null) {
        return <>
            <h1>PicStamper</h1>
            <button onClick={() => {
                getJob().then(r => setJob(r));
            }}>Start new processing job</button></>;
    } else {
        if (uploadFiles.length < 1) {
            return <UploadForm jobId={job.jobId} uploadUrl={"none"} setUploadFiles={setUploadFiles} />;
        } else if (!uploadFinished) {
            return <UploadScreen jobId={job.jobId} files={uploadFiles} setUploadFinished={setUploadFinished} />;
        } else {
            return <DownloadScreen jobId={job.jobId} />;
        }
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