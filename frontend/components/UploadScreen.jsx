import * as React from "react";
import { useState, useEffect } from "react";
import Axios from "axios";

export default function UploadScreen({ jobId, uploadUrl, files, setUploadFinished }) {
    let [currentFileNo, setCurrentFileNo] = useState(0);
    useEffect(() => {
        (async () => {
            for (let i = 0; i < files.length; i++) {
                setCurrentFileNo(i);
                console.log(`Uploading file #${i + 1}/${files.length}, ${files[i].name}...`);
                await Axios.put(uploadUrl.replace("PLACEHOLDER.jpg", files[i].name), files[i], {
                    headers: {
                        "content-type": "multipart/form-data"
                    }
                });
                console.log(`Uploaded file #${i + 1}/${files.length}, ${files[i].name}.`);
            }
            await Axios.post(`https://picstamper-api.oreganoli.xyz/startJob/${jobId}`);
        })().then(() => setUploadFinished(true));
    }, [files]);
    return <>
        <h1>Job #{jobId}</h1>
        <div className="alert alert-secondary d-flex flex-row justify-content-evenly align-items-center">
            {`Please wait, uploading file ${files[currentFileNo].name} (${currentFileNo + 1}/${files.length})... `}
            <div className="spinner-border" role="status "><span className="visually-hidden">Uploading...</span></div>
        </div>
    </>
}