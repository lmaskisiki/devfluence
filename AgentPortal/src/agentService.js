
function agentService(storageService) {
    let addViewModel = {
        addAgent: function (agent) {

        }
    }

    let addAgent = (agent) => {
        storageService.addAgent(agent);
        return "success";
    }
    
    return{
        addAgent:addAgent,
       
    }


}