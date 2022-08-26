import * as React from "react";
import { useState, useEffect } from "react";

export default function UploadScreen({ jobId, files }) {
    let [currentFileNo, setCurrentFileNo] = useState(0);
    useEffect(() => {
        if (currentFileNo == files.length - 1) {
            return;
        }
        console.log(`Uploading file #${currentFileNo}, ${files[currentFileNo].name}...`)
        new Promise(resolve => setTimeout(resolve, 2000)).then(() => setCurrentFileNo(x => x + 1));
    }, [currentFileNo]);
    return <>
        <h1>Job #{jobId}</h1>
        {(currentFileNo) < files.length ? <p>Please wait, uploading file {currentFileNo + 1}/{files.length}...</p> : <p>Upload complete.</p>}
    </>
}