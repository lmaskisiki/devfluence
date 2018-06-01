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
    self.errors = ko.observableArray([]);
    self.showScriptTextArea = ko.observable(false);


    self.historyShown = ko.observable(false);
    self.dashboardHistoryShown = ko.observable(false);

    self.executions = ko.observableArray([]);
    self.dashboardHistory = ko.observableArray([
        { target: "dashboad", action: "ip", result: "localhost", time: "2017-05-12" },
        { target: "Agent Smith", action: "os", result: "Windows 10", time: "2017-05-12" }
    ]);

    self.showDashboardHistory = function (show) {
        if(self.dashboardHistoryShown(show)){
            self.ShowAgent(!show);
            self.ShowAddAgent(false);
            self.ShowRemoveAgent(false);
            self.showExecuteAgent(false);
            self.ShowUpdateAgent(false);
            self.historyShown(false);
            self.getDashboardActivities();
        }
    }

    self.showHistory = function (show, agentName) {
        if (self.historyShown(show))
        {
            self.ShowAgent(!show);
            self.ShowAddAgent(false);
            self.ShowRemoveAgent(false);
            self.showExecuteAgent(false);
            self.getAgentExecutions(agentName);
        }
    }

    self.ShowAddAgentForm = function (show) {
        if (self.ShowAddAgent(show)) {
            self.ShowAgent(!show);
            self.ShowUpdateAgent(false);
            self.ShowRemoveAgent(false);
            self.showExecuteAgent(false);
            self.dashboardHistoryShown(false);
            self.historyShown(false);
        }
    }

    self.ShowUpdateAgentForm = function (show) {
        if (self.ShowUpdateAgent(show)) {
            self.ShowAgent(!show);
            self.ShowAddAgent(false);
            self.ShowRemoveAgent(false);
            self.showExecuteAgent(false);
            self.dashboardHistoryShown(false);
            self.historyShown(false);
        }
    }

    self.ShowRemoveAgentForm = function (show) {
        if (self.ShowRemoveAgent(show)) {
            self.ShowAgent(!show);
            self.ShowAddAgent(false);
            self.showExecuteAgent(false);
            self.ShowUpdateAgent(false);
            self.dashboardHistoryShown(false);
            self.historyShown(false);
        }
    }

    self.ShowExecuteAgentForm = function (show) {
        if (self.showExecuteAgent(show)) {
            self.ShowAgent(!show);
            self.ShowUpdateAgent(false);
            self.ShowRemoveAgent(false);
            self.ShowAddAgent(false);
            self.dashboardHistoryShown(false);
            self.historyShown(false);
        };
    }

    self.ShowAgentForm = function (show) {
        self.ShowAgent(show);
       
    }

    self.selectionChanged = function () {
        if (self.commandToRun() == 'script') {
            self.showScriptTextArea(true);
        } else {
            self.showScriptTextArea(false);
        }
    }

    self.removeAgent = function () {
         let agentName= self.activeAgent().name;
        self.agents.remove(self.activeAgent());
        self.refereshAgents();
        service.activityLogger("Dashboard", "Remove Agent", agentName + " Removed");
    }

    self.save = function (formElement) {
        let agentModel = new agent(formElement.name.value, formElement.ipAddress.value, formElement.port.value);
        let response = this.addAgent(agentModel);
        if (response) self.errors.push(response);
        self.ShowAddAgentForm(false);
    }

    self.update = function (formElement) {
        let availableAgents = new agent(formElement.name.value, formElement.ipAddress.value, formElement.port.value);
        let response = this.updateAgent(availableAgents);
        self.ShowUpdateAgentForm(false);
    }

    self.runCommand = function () {
        ko.utils.arrayForEach(self.activeAgents(), function (a) {
            let scriptData = `{"PowerShellScript":"${self.scriptData()}"}`;
            service.runCommand(self.commandToRun(), a, scriptData, function (response) {
                if (response.length > 0) {
                    let responsJson = JSON.parse(response);
                    let execution = {
                        command: self.commandToRun(),
                        output: (self.commandToRun() == "script") ? responsJson.output : responsJson.result,
                        time: new Date()
                    };

                    a.executions.push(execution);
                    service.executionLogger(a, execution.command, execution.output)
                    service.activityLogger(a.name, execution.command, execution.output)
                }
                self.refereshAgents();
            }, () => { });
        });
    }

    self.getAgentExecutions = function (agentName) {
        let agent = self.agents().find(a => a.name == agentName);
        if (agent) {
            agent.getExecutions((response, statusCode) => {
                if (statusCode == 200 && response.length > 0) {
                    jsonResponse = JSON.parse(response);
                    addResultToExecutions(jsonResponse);
                }
            }, (msg)=>{
                console.log(msg);
            });
        }
    }

    self.getDashboardActivities = function () {
        service.getDashboardActivities((response, statusCode) => {
            if (statusCode == 200 && response.length > 0) {
                jsonResponse = JSON.parse(response);
                addResultToDashboardHistory(jsonResponse);
            }
        }, (msg)=>{
            console.log(msg);
        });
    }
    let addResultToDashboardHistory = function (jsonResponse) {
        self.dashboardHistory([]);
        jsonResponse.forEach(e => {
            self.dashboardHistory.push({
                target: e.target,
                action: e.action,
                result: e.actionResult,
                time: e.executionTime
            })
        });
    }
    let addResultToExecutions = function (jsonResponse) {
        self.executions([]);
        jsonResponse.forEach(e => {
            self.executions.push({ command: e.command, result: e.result, utcTime: e.executionTime })
        });
    }

    self.refereshAgents = function () {
        let allAgents = self.agents();
        self.agents([]);
        self.agents(allAgents);
    }

    self.addAgent = function (agent) {
        if (isNotValid(agent) === true) return;
        service.ping(agent, function (text, statusCode) {
            if (statusCode === 200) {
                agent = self.setAgentStatusTo("ACTIVE", agent);
                self.addAgentToList(agent);
                service.activityLogger("Dashboard", "Add Agent", agent.name + " added");
            } else {
                service.activityLogger("Dashboard", "Add Agent", " Could Contact " + agent.name);
                self.errors.push(`Could not contact agent ${agent.name} on ${agent.ipAddress}:${agent.port}`);
            }
        }, (a) => {
            self.errors.push("Could not contact the agent");
            service.activityLogger("Dashboard", "Add Agent", "Could Contact " + agent.name);

        });
    }

    self.updateAgent = function (updatedAgent) {
        updatedAgent.canContact(() => {
            self.activeAgents()[0].name = updatedAgent.name;
            self.activeAgents()[0].ipAddress = updatedAgent.ipAddress;
            self.activeAgents()[0].port = updatedAgent.port;
            self.refereshAgents();
        }, () => {
            self.errors().push("Not updated");
        });

        self.refereshAgents();

    }

    self.getAgents = function () {
        return self.agents();
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

// function agent(name, ipAddress, port) {
//     let _service = new agentService();
//     let _name = name;
//     let _ipAddress = ipAddress;
//     let _port = port;
//     let _active = false;
//     return {
//         name: _name,
//         ipAddress: _ipAddress,
//         port: _port,
//         active: _active,
//         executions: [],
//         getExecutions: function () {
//             return _service.getExecutions(this, (response) => {
//                     JSON.parse(response).array.forEach(element => {
//                        // this.executions.push(element);
//                     });
//             }, () => {

//             });
//         },
//         canContact: function (doneFn, erroFn) {
//             _service.ping(this, doneFn, erroFn);
//         }
//     }
// }git


let service = agentService();
let viewModel = new addViewModel(service);

ko.applyBindings(viewModel);

