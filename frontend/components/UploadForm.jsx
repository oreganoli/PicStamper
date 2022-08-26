import * as React from "react";
import { useState } from "react";
export default function UploadForm({ jobId, uploadUrl }) {
    let [canStart, setCanStart] = useState(true);
    let [jobState, setJobState] = useState("pending");
    let [files, setFiles] = useState([]);
    let stateDescriptions = {
        "pending": "Start job",
        "inProgress": "Processing..."
    };
    let putFile = (newFiles) => {
        setFiles(files.concat(newFiles));
    }
    let removeFile = (index) => {
        let filesTemp = files;
        filesTemp.splice(index, 1);
        setFiles([...filesTemp]);
        console.log(`Removed file #${index}`);
    }
    return <>
        <h1>Job #{jobId}</h1>
        {files.map((file, i, _arr) => <FileEntry filename={file} index={i} key={i} removeFile={removeFile} />)}
        <FileUploader initialValue="" putFile={putFile} index={files.length} />
        <button disabled={!canStart || files.length < 1} onClick={() => {
            setCanStart(false);
            setJobState("inProgress");
        }}>{stateDescriptions[jobState]}</button>
    </>
}

function FileEntry({ filename, removeFile, index }) {
    return <div>{filename} <button onClick={() => removeFile(index)}>Remove</button></div>
}

function FileUploader({ putFile }) {
    return <div>
        <input id="addFilesInput" style={{ "display": "none" }} value="" type="file" onChange={e => {
            e.preventDefault();
            let filenames = [];
            for (let file of e.target.files) {
                filenames.push(file.name);
            }
            putFile(filenames);
        }}></input>
        <button onClick={() => document.getElementById("addFilesInput").click()}>Add files</button>
    </div >
}