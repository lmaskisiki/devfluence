
function addViewModel(service) {
    self.agents = ko.observableArray([]);

    self.ShowAddAgent  = ko.observable(false);
    self.ShowUpdateAgent = ko.observable(false);
    self.showExecuteAgent = ko.observable(false);

    self.ShowAddAgentForm = function(show){
        self.ShowAddAgent(show);
    }
    self.ShowUpdateAgentForm = function(show){
        self.ShowUpdateAgent(show);
    }

    self.ShowExecuteAgentForm = function(show){
        self.showExecuteAgent(show);
    }

    self.save = function (formElement) {
        let agentModel = new agent(formElement.name.value, formElement.ipAddress.value, formElement.port.value);
        self.agents.push(agentModel);
        self.ShowAddAgentForm(false);
        this.addAgent(agentModel);
    }
  
    this.addAgent = function (agent) {
        if (emptyObject(agent))
            return "EMPTY_OBJECT";
        let pingResult = service.ping(agent);
        if (pingResult === "SUCCESS") {
            return service.addAgent(agent);
        }
        return pingResult;
    }


    let emptyObject = (agent) => {
        if (agent.name.length === 0 | agent.ipAddress.length == 0 | agent.port === undefined) {
            return true;
        }
        return false;
    }
}

function agent(name, ipAddress, port) {
    let _name = name;
    let _ipAddress = ipAddress;
    let _port = port;
    return {
        name: _name,
        ipAddress: _ipAddress,
        port: _port
    }
}



