describe("getExecutionHistory", () => {
    beforeEach(() => {
        jasmine.Ajax.install();
    });
    afterEach(() => {
        jasmine.Ajax.uninstall();
    });

    describe("when history is avalable", () => {
        it("should save to agent exection array", () => {
            //Arrange
            let service = new agentService();
            let viewModel = new addViewModel(service);
            let agent = new agentTestBuilder()
                .withName("Agent 1")
                .withIpAddress("192.168.11.101")
                .withPort(8282)
                .build();

            let currentTestAgentExecututions =
                [
                    { command: 'ip', result: '172.168.11.2', executionTime: '2018/05/30 15:47:11 0000' },
                    { command: 'hostname', result: 'lizo-pc', executionTime: '2018/05/30 15:47:11 0000' }
                ];

            jasmine.Ajax.stubRequest(`http://${agent.ipAddress}:${agent.port}/api/agentHistory`).andReturn({
                "status": 200,
                "contentType": 'application/json',
                "responseText": JSON.stringify(currentTestAgentExecututions)
            });

            viewModel.agents()[0] = agent;
            //Act 
            viewModel.getAgentExecutions("Agent 1");

            //Assert
            expect(viewModel.executions()).toEqual(currentTestAgentExecututions);
        });
    });

    describe("when no history is avalable", () => {
        it("should keep agent execution array empty ", () => {
            //Arrange
            let service = new agentService();
            let viewModel = new addViewModel(service);
            let agent = new agentTestBuilder()
                .withName("Agent 1")
                .withIpAddress("192.168.11.101")
                .withPort(8282)
                .build();

            jasmine.Ajax.stubRequest(`http://${agent.ipAddress}:${agent.port}/api/agentHistory`).andReturn({
                "status": 200,
                "contentType": 'application/json',
                "responseText": ""
            });

            //Act
            viewModel.getAgentExecutions(agent);

            //Assert
            expect(agent.executions).toEqual([]);
        });
    });
});