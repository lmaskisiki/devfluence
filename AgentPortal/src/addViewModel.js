
function addViewModel(service) {

    let self = this;


    self.ShowAgents = ko.observable(false);
    self.ShowAddAgent = ko.observable(false);
    self.ShowUpdateAgent = ko.observable(false);
    self.showExecuteAgent = ko.observable(false);
    self.histories = ko.observableArray([]);
    self.activeAgent = ko.observable();
    self.commandToRun = ko.observable();

    self.ShowAddAgentForm = function (show) {
        self.ShowAddAgent(show);
    }
    self.ShowUpdateAgentForm = function (show) {
        self.ShowUpdateAgent(show);
    }

    self.ShowExecuteAgentForm = function (show) {
        self.showExecuteAgent(show);
    }

    self.agents = ko.observableArray([]);
    self.ShowAgentForm = function (show) {
        self.showAgents(show);
    }

    self.save = function (formElement) {
        let agentModel = new agent(formElement.name.value, formElement.ipAddress.value, formElement.port.value);
        this.addAgent(agentModel);
        self.ShowAddAgentForm(false);
    }

    self.runCommand = function () {
        service.runCommand(self.commandToRun(), self.activeAgent(), function (resp) {
            let selected = self.agents().find(a => a.name == self.activeAgent().name);
            let index = self.agents().indexOf(selected);
            self.activeAgent().execution.push(resp.output);
            alert(JSON.stringify(self.activeAgent()));
            self.agents()[index].execution.push(resp.output);
            self.agents(self.agents())
            //self.addAgentToList(self.activeAgent());
        });
    }

    this.addAgent = function (agent) {
        if (emptyObject(agent))
            return "EMPTY_OBJECT";
        return service.ping(agent, function () {
            agent.active = true;
            self.addAgentToList(agent);
        }, function (error) {
            agent.active = false;
            self.addAgentToList(agent);
        });
    }

    self.addAgentToList = function (agent) {
        self.agents().push(agent);
        self.agents(self.agents())
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
    let _active = false;
    return {
        name: _name,
        ipAddress: _ipAddress,
        port: _port,
        active: _active,
        execution: []
    }
}



