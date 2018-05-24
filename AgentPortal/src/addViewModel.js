function addViewModel(service) {

    let self = this;


    self.ShowAgents = ko.observable(false);
    self.ShowAddAgent = ko.observable(false);
    self.ShowUpdateAgent = ko.observable(false);
    self.ShowRemoveAgent = ko.observable(false);
    self.showExecuteAgent = ko.observable(false);
    self.histories = ko.observableArray([]);
    self.activeAgent = ko.observable();
    self.agents = ko.observableArray([]);

    self.ShowAddAgentForm = function (show) {
        self.ShowAddAgent(show);
    }
    self.ShowUpdateAgentForm = function (show) {
        self.ShowUpdateAgent(show);
    }

    self.ShowRemoveAgentForm = function (show) {
        self.ShowRemoveAgent(show);
    }

    self.ShowExecuteAgentForm = function (show) {
        self.showExecuteAgent(show);
    }


    self.ShowAgentForm = function (show) {
        self.showAgents(show);
    }

    self.save = function (formElement) {
        let agentModel = new agent(formElement.name.value, formElement.ipAddress.value, formElement.port.value);
        this.addAgent(agentModel);
        self.ShowAddAgentForm(false);
    }

    self.runCommand = function (command, agent) {
        let a = new agentTestBuilder()
            .withName("Agent Meeee")
            .withIpAddress("localhost")
            .withPort(1234)
            .build();
        service.runCommand("ip", self.activeAgent(), function (resp) {
            self.activeAgent.execution.push(resp.output);
            alert(self.agents()[0].executionResult);
        });
    }

    self.addAgent = function (agent) {
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
        self.agents.push(agent);
    }

    let emptyObject = (agent) => {
        if (agent.name.length === 0 | agent.ipAddress.length == 0 | agent.port === undefined) {
            return true;
        }
        return false;
    }

    self.removeAgent = function (agent) {
        self.agents.remove(self.activeAgent());
    }
}
// REmove
self.agentToRemove = ko.observableArray();



//
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