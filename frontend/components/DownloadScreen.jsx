import * as React from "react";
import { useState, useEffect } from "react";
import Axios from "axios";

export default function DownloadScreen({ jobId, resetCallback }) {
    let [downloadLink, setDownloadLink] = useState("");
    useEffect(() => {
        (async () => {
            while (true) {
                let response = await Axios.get(`https://picstamper-api.oreganoli.xyz/jobs/${jobId}`);
                let jobInfo = response.data;
                if (jobInfo.DownloadLink !== "") {
                    setDownloadLink(jobInfo.DownloadLink);
                    break;
                } else {
                    await new Promise(resolve => setTimeout(resolve, 5000));
                }
            }
        })().then(() => console.log("Processing complete."));
    }, [jobId]);
    if (downloadLink === "") {
        return <>
            <h1 className="">Job #{jobId}</h1>
            <div className="alert alert-secondary d-flex flex-row justify-content-evenly align-items-center">Upload complete. Please wait while your job is being processed... <div className="spinner-border" role="status "><span className="visually-hidden">Querying server...</span></div></div>
        </>;
    } else {
        return <>
            <h1 className="">Success</h1>
            <div className="alert alert-success">The job was finished.</div>
            <div className="d-flex flex-row justify-content-evenly align-items-center">
                <a id="downloadLink" href={downloadLink} download style={{ "display": "none" }}></a>
                <button className="btn btn-primary" onClick={() => document.getElementById("downloadLink").click()}>Download file</button>
                <button className="btn btn-secondary" onClick={() => {
                    if (window.confirm("Are you sure you want to start another job? You will not be able to download the output of this one.")) {
                        resetCallback();
                    }
                }}>Start another job</button>
            </div>
        </>
    }
}