import * as React from "react";
import { useState, useEffect } from "react"

async function getJobs() {
    let response = await fetch("https://picstamper-api.oreganoli.xyz/jobs");
    let json = await response.json();
    return json;
}

export function JobList() {
    let [jobs, setJobs] = useState([]);
    let [jobsSet, setJobsSet] = useState(false);
    useEffect(() => {
        console.debug("Getting list of jobs...");
        getJobs().then((list) => {
            setJobs(list)
            setJobsSet(true);
            console.log(jobs);
        });
    }, [jobsSet]);
    return jobs.map(each => {
        return <p>Job number {each.JobId}</p>;
    });
};