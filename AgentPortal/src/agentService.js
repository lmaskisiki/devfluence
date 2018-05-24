
function agentService(storageService) {

    function addAgent(agent) {
        storageService.addAgent(agent);
        return storageService.getAgents().includes(agent) ? "SUCCESS" : "ERROR";
    }

    function getAgents() {
        return storageService.getAgents();
    }

    function ping(agent, onSuccess, onError) {
        return doGet(agent.ipAddress, agent.port, "health")
            .then(function (result) {
                if (result.statusCode == 200) {
                    console.log("executing success");
                    return onSuccess();
                }
            }).catch(e => onError(e));
    }

    function doGet(ip, port, endpoint) {
        return handleHttp("GET", `http://${ip}:${port}/api/${endpoint}`);
    }


    function doGet2(ip, port, endpoint, doneFn) {

        return handleHttp2("GET", `http://${ip}:${port}/api/${endpoint}`, doneFn);
    }


    function runCommand(command, agent, doneFn) {
        if(command =="script"){
            handlePost("",`http://${agent.ip}:${agent.port}/api/script`, doneFn);
        }

        doGet2(agent.ipAddress, agent.port, command, doneFn)
    }

    function handlePost(data, url, doneFn) {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                doneFn(JSON.parse(xhttp.responseText))
            } else if (this.readyState == 4) {
                doneFn();
            }
        };
        console.log(url);
        xhttp.open("POST", url, true);
        xhttp.setRequestHeader("Content-type", "application/json");
        xhttp.send(data);
    }

    function handleHttp2(method, url, doneFn) {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                doneFn(JSON.parse(xhttp.responseText))
            } else if (this.readyState == 4) {
                doneFn();
            }
        };
        console.log(url);
        xhttp.open("GET", url, true);
        xhttp.setRequestHeader("Content-type", "application/json");
        xhttp.send();
    }

    function handleHttp(method, url) {
        console.log(url);
        return new Promise((resolve, reject) => {
            const xhr = new XMLHttpRequest();
            xhr.open(method, url);
            xhr.onload = () => resolve({ statusCode: xhr.status, data: xhr.responseText });
            xhr.onerror = () => reject({ statusCode: xhr.status, data: xhr.responseText });
            xhr.send();
        });
    }

    return {
        addAgent: addAgent,
        ping: ping,
        doGet, doGet,
        getAgents: getAgents,
        ping: ping,
        runCommand: runCommand
    }
}