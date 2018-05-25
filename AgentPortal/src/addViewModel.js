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

    self.ShowAddAgentForm = function (show) {
       return self.ShowAddAgent(show)
        // if (self.ShowAddAgent(show)) {
        //     //self.ShowAgent(!show);
        // }
    }

    self.ShowUpdateAgentForm = function (show) {
        self.ShowUpdateAgent(show);
        self.ShowAgent(!show);
    }

    self.ShowRemoveAgentForm = function (show) {
        if (self.ShowRemoveAgent(show)) {
            self.ShowAgent(show);
        }
    }

    self.ShowExecuteAgentForm = function (show) {
        self.showExecuteAgent(show);  
    }

    // self.ShowAgentForm = function (show) {
    //     self.showAgents(show);
    // }
    self.removeAgent = function (agent) {
        self.agents.remove(self.activeAgent());

    }

    self.save = function (formElement) {
        let agentModel = new agent(formElement.name.value, formElement.ipAddress.value, formElement.port.value);
        this.addAgent(agentModel);
        self.ShowAddAgentForm(false);
    }

    self.runCommand = function () {
        service.runCommand(self.commandToRun(), self.activeAgent(), function (response) {
            if (response.length > 0) {
                let responsJson = JSON.parse(response);
                self.activeAgent().execution.push(responsJson.output);
            }
            self.refereshAgents();
        }, self.scriptData());
    }

    self.refereshAgents = function () {
        let allAgents = self.agents(); //hack
        self.agents([]);
        self.agents(allAgents);
    }

    self.addAgent = function (agent) {
        if (emptyObject(agent))
            return "EMPTY_OBJECT";
        service.ping(agent, function (text, statusCode) {
            agent.active = (statusCode == 200) ? true : false;
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