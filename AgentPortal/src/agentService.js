
function agentService(storageService) {

    function addAgent(agent) {
       storageService.addAgent(agent);
       return storageService.getAgents().includes(agent) ? "SUCCESS": "ERROR";
    }

    function ping(agent) {
        return "SUCCESS";
    }

    let doPost = () => {
        // var xhttp = new XMLHttpRequest();
        // xhttp.onreadystatechange = function () {
        //     if (this.readyState == 4 && this.status == 200) {
        //         console.log("done", xhttp.responseText, this.statusText);
        //         return xhttp.statusText;
        //     }

        // };
        // xhttp.open("GET", "http://ff:1234/api/health", true);
        // xhttp.setRequestHeader("Content-Type", "application/json");
        // xhttp.setRequestHeader("Accept", "application/json");
        // xhttp.send();
    }

    return {
        addAgent: addAgent,
        doPost: doPost,
        ping: ping
    }


}