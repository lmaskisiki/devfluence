function storageService() {

    let agents = [];
    function addAgent(agent) {
         console.log("adding",agent);
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