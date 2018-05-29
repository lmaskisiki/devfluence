function addViewModel(service) {

    let self = this;

    self.ShowAgent = ko.observable(true);
    self.ShowAddAgent = ko.observable(false);
    self.ShowUpdateAgent = ko.observable(false);
    self.ShowRemoveAgent = ko.observable(false);
    self.showExecuteAgent = ko.observable(false);
    self.ShowScript = ko.observable(false);

    self.histories = ko.observableArray([]);
    self.activeAgent = ko.observable();
    self.commandToRun = ko.observable();
    self.agents = ko.observableArray([]);
    self.scriptData = ko.observable('');
    self.agentToRemove = ko.observable();
    self.activeAgents = ko.observableArray([]);
    self.errors = ko.observableArray([""]);


    self.ShowAddAgentForm = function (show) {
        if (self.ShowAddAgent(show)) {
            self.ShowAgent(!show);
            self.ShowUpdateAgent(false);
            self.ShowRemoveAgent(false);
            self.showExecuteAgent(false);
        }
    }

    self.ShowUpdateAgentForm = function (show) {
        if (self.ShowUpdateAgent(show)) {
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
        let response = this.addAgent(agentModel);
        if (response) self.errors.push(response);
        self.ShowAddAgentForm(false);
    }

    self.runCommand = function () {
        ko.utils.arrayForEach(self.activeAgents(), function (a) {
            service.runCommand(self.commandToRun(), a, self.scriptData(), function (response) {
               
                if (response.length > 0) {
                    let responsJson = JSON.parse(response);
                    let execution = {
                        command: self.commandToRun(),
                        output: (self.commandToRun() == "script" )? responsJson.output : responsJson.result,
                        time: new Date()
                    };
                    a.execution.push(execution);
                }
                

                self.refereshAgents();
            }, () => { });
        });
    }

    var vm = {
        myItems: [
            {  commandToRun: 'script', disable: ko.observable(true)}
        ],
        setOptionDisable: function(option, item) {
            ko.applyBindingsToNode(option, {disable: item.disable}, item);
        }
    };
    //ko.applyBindings(vm);

    self.refereshAgents = function () {
        let allAgents = self.agents(); //hack
        self.agents([]);
        self.agents(allAgents);
    }

    self.addAgent = function (agent) {
        if (isNotValid(agent) === true) return;
        service.ping(agent, function (text, statusCode) {
            if (statusCode === 200) {
                agent = self.setAgentStatusTo("ACTIVE", agent);
                self.addAgentToList(agent);
            } else {
                self.errors.push(`Could not contact agent ${agent.name} on ${agent.ipAddress}:${agent.port}`);
            }
        }, (a) => { self.errors.push("Could not contact the agent") });
    }

    self.setAgentStatusTo = (status, agent) => {
        agent.active = (status === "ACTIVE") ? true : false;
        return agent;
    }

    self.addAgentToList = function (agent) {
        self.agents().push(agent);
        self.agents(self.agents())
    }

    let isNotValid = (agent) => {
        if (emptyObject(agent)) {
            self.errors.push("EMPTY OBJECT");
            return true;
        }
        if (duplicatedAgent(agent)) {
            self.errors.push("DUPLICATE AGENT");
            return true;
        }
        return false;
    }

    let duplicatedAgent = function (newAgent) {
        if (self.agents().length > 0) {
            var agentFound = self.agents().find((a) => a.name === newAgent.name | a.ipAddress == newAgent.ipAddress);
            if (agentFound) return true;
        }
        return false;
    }

    let emptyObject = (agent) => {
        if (agent.name.length === 0 | agent.ipAddress.length == 0 | agent.port === undefined) {
            return true;
        }
        return false;
    }

    //UI CONTROLS
    window.setInterval(function () {
        self.errors.removeAll();
    }, 7000);

    window.setInterval(function () {
        // for each agent on the view model
        ko.utils.arrayForEach(self.agents(), (agent) => {

            let successFn = function () {
                agent.active = true;
            };

            let errorFn = function () {
                console.log("error function executed...");
                agent.active = false;
            };
            self.refereshAgents();
            agent.canContact(successFn, errorFn);
        });
    }, 10000);
}

function agent(name, ipAddress, port) {
    let _service = new agentService();
    let _name = name;
    let _ipAddress = ipAddress;
    let _port = port;
    let _active = false;
    return {
        name: _name,
        ipAddress: _ipAddress,
        port: _port,
        active: _active,
        execution: [],
        canContact: function (doneFn, erroFn) {
            _service.ping(this, doneFn, erroFn);
        }
    }
}


