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
            return <UploadForm jobId={job.jobId} setUploadFiles={setUploadFiles} />;
        } else if (!uploadFinished) {
            return <UploadScreen jobId={job.jobId} uploadUrl={job.url} files={uploadFiles} setUploadFinished={setUploadFinished} />;
        } else {
            return <DownloadScreen jobId={job.jobId} resetCallback={() => {
                setJob(null);
                setUploadFiles([]);
                setUploadFinished(false);
            }} />;
        }
    }
}

async function getJob() {
    return await fetch("https://picstamper-api.oreganoli.xyz/newJob", {
        method: "POST"
    }).then(success => success.json())
}
// function 