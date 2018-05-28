describe("addViewModel", () => {
    describe("addAgent", () => {
        describe("When agent name, ip and port are empty", () => {
            it("Should return EMPTY_OBJECT, and not call agentService", () => {
                //Arrange
                let service = new agentService();
                let viewModel = new addViewModel(service);
                let agent = new agentTestBuilder()
                    .withName("")
                    .withIpAddress("")
                    .withPort()
                    .build();

                //Act
                const result = viewModel.addAgent(agent);
                //Assert
                expect(result).toBe("EMPTY_OBJECT");
            });
        });

        describe("Given agent name, ip address and port number", () => {
            describe("When agent is not reachable", () => {
                beforeEach(() => {
                    jasmine.Ajax.install();
                });
                afterEach(() => {
                    jasmine.Ajax.uninstall();
                });
                it("Should not add agent to list", () => {
                    //Arrange
                    let service = new agentService();
                    let viewModel = new addViewModel(service);
                    let agent = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .withPort(8282)
                        .build();

                    jasmine.Ajax.stubRequest('http://192.168.11.101:8282/api/health').andReturn({
                        "status": 404,
                        "contentType": 'application/json',
                        "responseText": 'Hello from the world'
                    });

                    spyOn(service, 'ping').and.callThrough();
                    spyOn(viewModel, 'addAgentToList');

                    //Act
                    let result = viewModel.addAgent(agent);

                    //Assert
                    expect(service.ping).toHaveBeenCalled();
                    expect(viewModel.addAgentToList).not.toHaveBeenCalled();
                    expect(viewModel.agents()).toEqual([]);
                });
            });

            describe("When agent is reachable", () => {
                beforeEach(() => {
                    jasmine.Ajax.install();
                });
                afterEach(() => {
                    jasmine.Ajax.uninstall();
                });
                it("Should agent to list", () => {
                    //Arrange
                    let service = new agentService();
                    let viewModel = new addViewModel(service);
                    let agent = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .withPort(8282)
                        .build();

                    jasmine.Ajax.stubRequest(`http://${agent.ipAddress}:${agent.port}/api/health`).andReturn({
                        "status": 200,
                        "contentType": 'application/json',
                        "responseText": 'Hello from the world'
                    });
                    spyOn(service, 'ping').and.callThrough();
                    spyOn(viewModel, 'setAgentStatusTo').and.callThrough();
                    spyOn(viewModel, 'addAgentToList');

                    //Act
                    let result = viewModel.addAgent(agent);

                    //Assert
                    expect(service.ping).toHaveBeenCalled();
                    expect(viewModel.setAgentStatusTo).toHaveBeenCalledWith('ACTIVE', agent);
                    expect(viewModel.addAgentToList).toHaveBeenCalledWith(agent);
                });

            });
            fdescribe("If Agent name or IP exists", function () {
                it("Should display error message", function () {
                    //Arrange 
                    let viewModel = new addViewModel();

                    let agent1 = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .build();
                    let agent2 = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .build();

                    viewModel.agents = [agent1];

                    // let result2 = viewModel.agents = [agent2];
                    // let final = result === result2;
                    // console.log(final)

                    //Act
                    result = viewModel.addAgent(agent2);

                    //Assert
                    expect(result).toBe("Duplicated_Agent")
                });
            });
        });
    });
});