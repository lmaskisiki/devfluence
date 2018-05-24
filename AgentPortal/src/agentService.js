function agentService(storageService) {
    let addViewModel = {
        addAgent: function (agent) {

        }
    }

    let addAgent = (agent) => {
        storageService.addAgent(agent);
        return "success";
    }
    let updateViewModel = {
        updateAgent: function (agent) {

        }
    }
    let updateAgent = (agent) => {
        storageService.updateAgent(agent);
        return "Successfully updated";
    }
    let removeViewModel = {
        removeAgent: function (agent) {

        }
    }
    let removeAgent = (agent) => {
        storageService.removeAgent(agent);
        return "Successfully removed";
    }

    return {
        addAgent: addAgent,
        updateAgent: updateAgent,
        removeAgent: removeAgent
    }

    

}