 function removeViewModel() {
     let self = this;
     self.agentToRemove = ko.observableArray();

     self.availableAgents = ko.observableArray([{
             agentName: "Agent1",
             ipAddress: "192.168.11.101",
             port: 8282
         },
         {
             agentName: "Agent2",
             ipAddress: "192.168.11.102",
             port: 8080
         },
         {
             agentName: "Agent3",
             ipAddress: "192.168.11.103",
             port: 8181
         }
     ]);

     self.removeAgent = function (agent) {
         let name = agent[0].selectedOptions[0].innerText;
         let agents = self.availableAgents().filter(a => a.agentName != name);
         self.availableAgents(agents);
     }
 }