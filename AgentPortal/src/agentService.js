

function agentService() {

    function ping(agent, onSuccess, erroFn) {
        return runCommand("health", agent, null, onSuccess, erroFn);
    }

    function runCommand(command, agent, data, doneFn, erroFn) {
        let method = (command === "script") ? "POST" : "GET";
        if (command === "fully-qualified hostname") {
            command = "hostname?fully-qualified=true";
        }
        return handleRequest(`http://${agent.ipAddress}:${agent.port}/api/${command}`, method, data, doneFn, erroFn);
    }

    function getExecutions(agent, doneFn, erroFn) {
        return handleRequest(`http://${agent.ipAddress}:${agent.port}/api/agentHistory`, "GET", null,doneFn, erroFn)
    }

    function handleRequest(url, method, data, doneFn, erroFn) {
         console.log(url);
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                doneFn(xhttp.responseText, xhttp.status)
            } else if (this.readyState == 4) {
                erroFn(xhttp.responseText, xhttp.status); //
            }
        };

        xhttp.open(method, url, true);
        xhttp.setRequestHeader("Content-Type", "application/json");
        xhttp.setRequestHeader("Accept", "application/json");
        xhttp.send(data);
    }

    return {
        ping: ping,
        getExecutions:getExecutions,
        runCommand: runCommand
    }
}
