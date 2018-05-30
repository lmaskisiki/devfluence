describe("addViewModel", () => {
    describe("addAgent", () => {
        describe("When agent name, ip and port are empty", () => {
            it("Should push 'EMPTY_OBJECT' to erros to be displayed on the screen, and not ping the agent", () => {
                //Arrange
                let service = new agentService();
                let viewModel = new addViewModel(service);
                let agent = new agentTestBuilder()
                    .withName("")
                    .withIpAddress("")
                    .withPort()
                    .build();
                spyOn(service, 'ping');
                //Act
                const result = viewModel.addAgent(agent);
                //Assert
                expect(viewModel.errors()).toContain("EMPTY OBJECT");
                expect(service.ping).not.toHaveBeenCalled();
            });
        });

        describe("Given agent name, ip address and port number", () => {

            beforeEach(() => {
                jasmine.Ajax.install();
            });
            afterEach(() => {
                jasmine.Ajax.uninstall();
            });

            describe("When agent is not reachable", () => {
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
            describe("If Agent name or IP exists", function () {
                it("Should display error message 'DUPLICATE AGENT'", function () {
                    //Arrange
                    let service = new agentService();
                    let viewModel = new addViewModel(service);
                    let agent1 = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .withPort(8282)
                        .build();

                    let agent2 = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .withPort(8282)
                        .build();

                    jasmine.Ajax.stubRequest(`http://${agent1.ipAddress}:${agent1.port}/api/health`).andReturn({
                        "status": 200,
                        "contentType": 'application/json',
                        "responseText": 'Hello from the world'
                    });

                    spyOn(service, 'ping').and.callThrough();

                    //Act
                    viewModel.agents()[0] = agent1;
                    viewModel.addAgent(agent2);

                    //Assert
                    expect(viewModel.errors).toContain('DUPLICATE AGENT');
                    expect(service.ping).not.toHaveBeenCalled();
                });
            });

            describe("If script is inputed", function(){
                xit("Should reformat the scrit", function(){
                    //Arrange
                    let viewModel = new addViewModel();

                    //Act

                    viewModel.runCommand(scriptData);
                    //Assert

                });
            });
        });
    });
});