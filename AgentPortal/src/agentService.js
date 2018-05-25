

function agentService(storageService) {

    function addAgent(agent) {
        storageService.addAgent(agent);
        return storageService.getAgents().includes(agent) ? "SUCCESS" : "ERROR";
    }

    function ping(agent, onSuccess) {
        return runCommand("health", agent, onSuccess, null);
    }

    function runCommand(command, agent, doneFn, data) {
        let method = (command === "script") ? "POST" : "GET";
        return handleRequest(`http://${agent.ipAddress}:${agent.port}/api/${command}`, doneFn, method, data);
    }

    function handleRequest(url, doneFn, method, data) {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                doneFn(xhttp.responseText, xhttp.status)
            } else if (this.readyState == 4) {
                doneFn(xhttp.responseText, xhttp.status);
            }
        };

        xhttp.open(method, url, true);
        xhttp.setRequestHeader("Content-Type", "application/json");
        xhttp.setRequestHeader("Accept", "application/json");
        xhttp.send(data);
    }

    return {
        addAgent: addAgent,
        ping: ping,
        runCommand: runCommand
    }
}
