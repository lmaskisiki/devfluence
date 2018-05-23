
function addViewModel(service) {
    self.agents = ko.observableArray();

    self.save = function (formElement) {
        let agentModel = new agent(formElement.name.value, formElement.ipAddress.value, formElement.port.value);
        this.addAgent(agentModel);
        self.agents.removeAll();
        ko.utils.arrayPushAll(self.agents, service.getAgents());
    }


    this.addAgent = function (agent) {
        if (emptyObject(agent))
            return "EMPTY_OBJECT";
        service.ping(agent).then(function (result) {
            if (result.statusCode === 200) {
                agent.active = true;
                service.addAgent(agent);
            } 
        }).catch(e => {
            agent.active = false;
            service.addAgent(agent);
        });
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
    let _active=false;
    return {
        name: _name,
        ipAddress: _ipAddress,
        port: _port,
        active:_active
    }
}


