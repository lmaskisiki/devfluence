
function agentService(storageService) {

    function addAgent(agent) {
        storageService.addAgent(agent);
        return storageService.getAgents().includes(agent) ? "SUCCESS" : "ERROR";
    }

    function ping(agent) {
         return doGet(agent.ipAddress, agent.port, "health");
    }

    async function doGet(ip, port, endpoint) {
       let resp = await handleHttp("GET", `http://${ip}:${port}/api/${endpoint}`);
       return Promise.resolve(resp);
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
        doGet, doGet
    }


}