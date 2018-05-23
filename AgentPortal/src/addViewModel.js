
function addViewModel(service) {

    self.save = function (formElement) {
        let agentModel = new agent(formElement.name.value, formElement.ipAddress.value, formElement.port.value);
        this.addAgent(agentModel);
    }

    this.addAgent = function (agent) {
        if (emptyObject(agent))
            return "EMPTY_OBJECT";
        service.ping(agent).then(function (result) {
            if (result.statusCode === 200) {
                service.addAgent(agent);
                return "SUCCESS";
            }
        }).catch(e=>console.log("error", e));
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


