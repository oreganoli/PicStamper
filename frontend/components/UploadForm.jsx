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
    let putFiles = (newFiles) => {
        let newFilesArr = [];
        // make a FileList into an array of Files
        for (let f of newFiles) {
            newFilesArr.push(f);
        }
        setFiles(files.concat(newFilesArr));
    }
    let removeFile = (index) => {
        files.splice(index, 1);
        setFiles([...files]);
        console.log(`Removed file #${index}`);
    }
    return <>
        <h1>Job #{jobId}</h1>
        {files.map((file, i, _arr) => <FileEntry filename={file.name} index={i} key={i} removeFile={removeFile} />)}
        <FilePicker initialValue="" putFiles={putFiles} index={files.length} />
        <button disabled={!canStart || files.length < 1} onClick={() => {
            setCanStart(false);
            setJobState("inProgress");
        }}>{stateDescriptions[jobState]}</button>
    </>
}

function FileEntry({ filename, removeFile, index }) {
    return <div>{filename} <button onClick={() => removeFile(index)}>Remove</button></div>
}

function FilePicker({ putFiles }) {
    return <div>
        <input accept=".jpg,.jpeg" multiple id="addFilesInput" style={{ "display": "none" }} value="" type="file" onChange={e => {
            e.preventDefault();
            putFiles(e.target.files);
        }}></input>
        <button onClick={() => document.getElementById("addFilesInput").click()}>Add files</button>
    </div >
}