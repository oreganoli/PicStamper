import * as React from "react";
import { useState, useEffect } from "react";

export default function DownloadScreen({ jobId }) {
    let [ticker, setTicker] = useState(0);
    let [downloadLink, setDownloadLink] = useState("");
    useEffect(() => {
        (async () => {
            for (let i = 0; i < 5; i++) {
                await new Promise(resolve => setTimeout(resolve, 2000));
                setTicker(x => x + 1);
            }
        })().then(() => setDownloadLink("test"));
    }, [jobId]);
    if (downloadLink === "") {
        return <>
            <h1>Job #{jobId}</h1>
            <p>Upload complete. Please wait while your job is being processed{ticker % 2 == 0 ? "..." : ".."}</p>
        </>;
    } else {
        return <>
            <h1>Success</h1>
            <h2>The job was finished.</h2>
        </>
    }
}