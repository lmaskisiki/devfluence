
function addViewModel() {

    this.addAgent = function (agent) {
        const agentService = this.getAgentService();
        if (emptyObject(agent))
            return "EMPTY_OBJECT";
        return agentService.addAgent(agent);
    }

    this.getAgentService = function () {
        return new agentService(new storageService());
    }

    let emptyObject = (agent) => {
        if (agent.name.length === 0 | agent.ipAddress.length == 0 | agent.port === undefined) {
            return true;
        }
        return false;
    }
}

var viewModel = new addViewModel();
ko.applyBindings(viewModel);
