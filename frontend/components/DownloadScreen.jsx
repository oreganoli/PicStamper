import * as React from "react";
import { useState, useEffect } from "react";
import Axios from "axios";

export default function DownloadScreen({ jobId, resetCallback }) {
    let [ticker, setTicker] = useState(0);
    let [downloadLink, setDownloadLink] = useState("");
    useEffect(() => {
        (async () => {
            while (true) {
                setTicker(x => x + 1);
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
        return <div className="container">
            <h1 className="text-center">Job #{jobId}</h1>
            <div className="alert alert-secondary">Upload complete. Please wait while your job is being processed{ticker % 2 == 0 ? "..." : ".."}</div>
        </div>;
    } else {
        return <div className="container">
            <h1 className="text-center">Success</h1>
            <div className="text-center alert alert-success">The job was finished.</div>
            <div className="d-flex flex-row justify-content-evenly align-items-center">
                <a id="downloadLink" href={downloadLink} download style={{ "display": "none" }}></a>
                <button className="text-center btn btn-primary" onClick={() => document.getElementById("downloadLink").click()}>Download file</button>
                <button className="text-center btn btn-secondary" onClick={() => {
                    if (window.confirm("Are you sure you want to start another job? You will not be able to download the output of this one.")) {
                        resetCallback();
                    }
                }}>Start another job</button>
            </div>
        </div>
    }
}