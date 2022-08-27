import * as React from "react";
import { useState, useEffect } from "react";

export default function UploadScreen({ jobId, files, setUploadFinished }) {
    let [currentFileNo, setCurrentFileNo] = useState(0);
    useEffect(() => {
        (async () => {
            for (let i = 0; i < files.length; i++) {
                setCurrentFileNo(i);
                console.log(`Uploading file #${i + 1}/${files.length}, ${files[i].name}...`);
                await new Promise(resolve => setTimeout(resolve, 2000));
                console.log(`Uploaded file #${i + 1}/${files.length}, ${files[i].name}.`);
            }
        })().then(() => setUploadFinished(true));
    }, [files]);
    return <>
        <h1>Job #{jobId}</h1>
        {(currentFileNo) < files.length ? <p>{`Please wait, uploading file ${files[currentFileNo].name}(${currentFileNo + 1}/${files.length})...`}</p> : <p>Upload complete.</p>}
    </>
}