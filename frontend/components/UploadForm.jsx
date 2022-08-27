import * as React from "react";
import { useState } from "react";

export default function UploadForm({ jobId, setUploadFiles }) {
    let [files, setFiles] = useState([]);
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
        setFiles([...files]);;
        console.log(`Removed file #${index}`);
    }
    return <>
        <h1>Job #{jobId}</h1>
        {files.map((file, i, _arr) => <FileEntry filename={file.name} index={i} key={i} removeFile={removeFile} />)}
        <FilePicker putFiles={putFiles} index={files.length} />
        <button disabled={files.length < 1} onClick={() => setUploadFiles(files)}>Start job</button>
    </>
}

function FileEntry({ filename, removeFile, index }) {
    return <div>{filename} <button onClick={() => removeFile(index)}>Remove</button></div>;
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

async function upload(_uploadLink, files, setUploadNum, setJobState) {
    for (let i = 0; i < files.length; i++) {
        console.log(`Uploading file #${i}, ${files[i].name}...`)
        await new Promise(resolve => setTimeout(resolve, 2000));
        setUploadNum(i + 1);
    }
    setJobState("processing");
}

function uploadButtonDescription(state, number, maxNumber) {
    if (state === "pending") {
        return "Start job";
    } else if (state === "inProgress") {
        return `Please wait, uploading file ${number}/${maxNumber}...`;
    } else if (state == "processing") {
        return "Please wait, processing...";
    }
}