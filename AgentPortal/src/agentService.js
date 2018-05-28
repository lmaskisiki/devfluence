

function agentService() {

    function ping(agent, onSuccess, erroFn) {
        console.log("now pinging " + JSON.stringify(agent));
        return runCommand("health", agent, null, onSuccess, erroFn);
    }

    function runCommand(command, agent, data, doneFn, erroFn) {
        let method = (command === "script") ? "POST" : "GET";
        return handleRequest(`http://${agent.ipAddress}:${agent.port}/api/${command}`,method, data, doneFn, erroFn);
    }

    function handleRequest(url,method,data, doneFn, erroFn) {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                doneFn(xhttp.responseText, xhttp.status)
            } else if (this.readyState == 4) {
                erroFn(xhttp.responseText, xhttp.status); // error function
            }
        };

        xhttp.open(method, url, true);
        xhttp.setRequestHeader("Content-Type", "application/json");
        xhttp.setRequestHeader("Accept", "application/json");
        xhttp.send(data);
    }

    return {
        ping: ping,
        runCommand: runCommand
    }
}
