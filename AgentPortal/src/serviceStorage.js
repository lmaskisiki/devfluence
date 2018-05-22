function storageService() {

    let agents = [];
    function addAgent(agent) {
         console.log("saving", agent);
        agents.push(agent);
    }

    function getAgents() {
        return agents;
    }

    return {
        addAgent: addAgent,
        getAgents: getAgents
    }
}