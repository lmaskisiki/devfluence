function storageService() {

    let agents = ["ddd"];
    function addAgent(agent) {
         console.log("all agent", agents, agents.length);
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