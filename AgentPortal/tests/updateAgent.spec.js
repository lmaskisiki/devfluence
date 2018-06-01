describe("addViewModel", function () {
    beforeEach(() => {
        jasmine.Ajax.install();
    });
    afterEach(() => {
        jasmine.Ajax.uninstall();
    });
    describe("updateAgent", () => {
        describe("When new deatials are valid and reachable", () => {
            fit("Should update agent details ", () => {
                //Arrange
                let service = new agentService();
                let viewModel = new addViewModel(service);

                let agent = new agentTestBuilder()
                    .withName("Agent 1")
                    .withIpAddress("192.168.11.101")
                    .withPort(8282)
                    .build();

                let updatedAgent = new agentTestBuilder()
                    .withName("Agent 2")
                    .withIpAddress("192.168.11.102")
                    .withPort(8181)
                    .build();

                jasmine.Ajax.stubRequest(`http://192.168.11.102:8181/api/health`).andReturn({
                    "status": 200,
                    "contentType": 'application/json',
                    "responseText": 'Succesfully updated'
                });

                viewModel.agents()[0] = agent;
                viewModel.activeAgents()[0] = agent;

                //Act
                viewModel.updateAgent(updatedAgent);

                //Assert
                expect(agent.name).toBe(updatedAgent.name);
                expect(agent.ipAddress).toBe(updatedAgent.ipAddress);
                expect(agent.port).toBe(updatedAgent.port);
            });
        });

        describe("When new deatials are not reachable", () => {
            fit("Should not update agent details ", () => {
                //Arrange
                let service = new agentService();
                let viewModel = new addViewModel(service);

                let agent = new agentTestBuilder()
                    .withName("Agent 1")
                    .withIpAddress("192.168.11.101")
                    .withPort(8282)
                    .build();

                let updatedAgent = new agentTestBuilder()
                    .withName("Agent 2")
                    .withIpAddress("192.168.11.102")
                    .withPort(8181)
                    .build();

                jasmine.Ajax.stubRequest(`http://192.168.11.102:8181/api/health`).andReturn({
                    "status": 400,
                    "contentType": 'application/json',
                    "responseText": 'Succesfully updated'
                });

                viewModel.agents()[0] = agent;
                viewModel.activeAgents()[0] = agent;

                //Act
                viewModel.updateAgent(updatedAgent);

                //Assert
                expect(agent.name).toBe("Agent 1");
                expect(agent.ipAddress).toBe("192.168.11.101");
                expect(agent.port).toBe(8282);
            });
        });
    });
});