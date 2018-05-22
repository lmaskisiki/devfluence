
function addViewModel() {
    let self= this;
  
    this.name = ko.observable("namedd");
    this.ipAddress= ko.observable("ip addres");
    this.port= ko.observable(0);
    
    self.save = function() {
         let agent = {};
         agent.name = this.name();
         agent.ipAddress = this.ipAddress();
         agent.port = this.port();
         this.addAgent(agent);
    }

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
