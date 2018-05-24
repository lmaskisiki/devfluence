function storageService() {

    let agents = [];
    function addAgent(agent) {
         agents.push(agent);
    }

    function getAgents() {
        return agents;
    }

    function updateAgent(agent){
        console.log("updating", agent);
        agents.push(agent);
    }

    function removeAgent(agent){
         agents.splice(agent);
    }

    return {
        addAgent: addAgent,
        getAgents: getAgents,
        updateAgent:updateAgent,
        removeAgent:removeAgent
    }
}