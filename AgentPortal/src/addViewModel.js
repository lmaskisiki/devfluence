function addViewModel(service) {

    let self = this;

    self.ShowAgent = ko.observable(true);
    self.ShowAddAgent = ko.observable(false);
    self.ShowUpdateAgent = ko.observable(false);
    self.ShowRemoveAgent = ko.observable(false);
    self.showExecuteAgent = ko.observable(false);

    self.histories = ko.observableArray([]);
    self.activeAgent = ko.observable();
    self.commandToRun = ko.observable();
    self.agents = ko.observableArray([]);
    self.scriptData = ko.observable('');
    self.agentToRemove = ko.observable();
    self.activeAgents = ko.observableArray([]);

    self.ShowAddAgentForm = function (show) {
        if (self.ShowAddAgent(show)) {
            self.ShowAgent(!show);
            self.ShowUpdateAgent(false);
            self.ShowRemoveAgent(false);
            self.showExecuteAgent(false);
        }
    }

    self.ShowUpdateAgentForm = function (show) {
        if(self.ShowUpdateAgent(show)){
            self.ShowAgent(!show);
            self.ShowAddAgent(false);
            self.ShowRemoveAgent(false);
            self.showExecuteAgent(false);
        }
    }

    self.ShowRemoveAgentForm = function (show) {
        if (self.ShowRemoveAgent(show)) {
            self.ShowAgent(!show);
            self.ShowAddAgent(false);
            self.showExecuteAgent(false);
            self.ShowUpdateAgent(false);
        }
    }

    self.ShowExecuteAgentForm = function (show) {
        if (self.showExecuteAgent(show)) {
            self.ShowAgent(!show);
            self.ShowUpdateAgent(false);
            self.ShowRemoveAgent(false);
            self.ShowAddAgent(false);
        };
    }

    self.ShowAgentForm = function (show) {
        self.ShowAgent(show);
    }

    self.removeAgent = function (agent) {
        self.agents.remove(self.activeAgent());
    }

    self.save = function (formElement) {
        let agentModel = new agent(formElement.name.value, formElement.ipAddress.value, formElement.port.value);
        this.addAgent(agentModel);
        self.ShowAddAgentForm(false);
    }

    self.runCommand = function () {
        ko.utils.arrayForEach(self.activeAgents(), function (a) {
            service.runCommand(self.commandToRun(), a, function (response) {
                if (response.length > 0) {
                    let responsJson = JSON.parse(response);
                    let execution = {
                        command: self.commandToRun(),
                        output: responsJson.output,
                        time: new Date()
                    };
                    a.execution.push(execution);
                }
                self.refereshAgents();
            }, self.scriptData());
        });
    }

    self.refereshAgents = function () {
        let allAgents = self.agents(); //hack
        self.agents([]);
        self.agents(allAgents);
    }

    self.addAgent = function (agent) {

        if (emptyObject(agent)){
            return "EMPTY_OBJECT";
        }
        if (duplicatedAgent(agent)){
            alert(" duplicated agent found..");
        return "Duplicated_Agent";
        }

        service.ping(agent, function (text, statusCode) {
            if (statusCode === 200) {
                agent = self.setAgentStatusTo("ACTIVE", agent);
                self.addAgentToList(agent);
            }
        });
    }

    self.setAgentStatusTo = (status, agent) => {
        agent.active = (status === "ACTIVE") ? true : false;
        return agent;
    }

    self.addAgentToList = function (agent) {
        self.agents().push(agent);
        self.agents(self.agents())
    }

    let duplicatedAgent = function (newAgent) {
        if (self.agents().length > 0) {
            var agentFound = self.agents().find((a) => a.name === newAgent.name | a.ipAddress == newAgent.ipAddress);
            if (agentFound) {
                return true;
            }
        }
        return false;
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
