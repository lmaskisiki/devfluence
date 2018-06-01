describe("runCommand", () => {
    beforeEach(() => {
        jasmine.Ajax.install();
    });
    afterEach(() => {
        jasmine.Ajax.uninstall();
    });
    describe("when command executed", () => {
        [{cmd:"ip",result:"192.168.11.101"},{cmd:"os",result:"192.168.11.101"}].forEach(command=>{
            it("should save the command and execution results", () => {
                //Arrange
                let service = new agentService();
                let viewModel = new addViewModel(service);
                let agent = new agentTestBuilder()
                    .withName("Agent 1")
                    .withIpAddress("192.168.11.101")
                    .withPort(8282)
                    .build();
    
    
                jasmine.Ajax.stubRequest(`http://${agent.ipAddress}:${agent.port}/api/${command.cmd}`).andReturn({
                    "status": 200,
                    "contentType": 'application/json',
                    "responseText": '{"result":"192.168.11.101"}'
                });
    
                spyOn(service, 'executionLogger');
    
                viewModel.agents()[0] = agent;
                viewModel.activeAgents()[0] = viewModel.agents()[0];
                viewModel.commandToRun(command.cmd);
                viewModel.runCommand();
    
                //Act
                expect(service.executionLogger).toHaveBeenCalledWith(agent,command.cmd,command.result);
    
            });
        });
       
    });
});