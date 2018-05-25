describe("addViewModel", () => {
    describe("addAgent", () => {
        describe("When agent name, ip and port are empty", () => {
            it("Should return EMPTY_OBJECT, and not call agentService", () => {
                //Arrange
                let storage = new storageService();
                let service = new agentService(storage);
                let viewModel = new addViewModel(service);
                let agent = new agentTestBuilder()
                    .withName("")
                    .withIpAddress("")
                    .withPort()
                    .build();

                spyOn(service, 'addAgent').and.returnValue('***');

                //Act
                const result = viewModel.addAgent(agent);
                //Assert
                expect(service.addAgent).not.toHaveBeenCalled();
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
                it("Should return error message 'NOT_FOUND'", () => {
                    //Arrange
                    let service = new agentService();
                    let viewModel = new addViewModel(service);
                    let agent = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .withPort(8282)
                        .build();

                    jasmine.Ajax.stubRequest('http://192.168.11.101:8282/api/health').andReturn({
                        "status": 400,
                        "contentType": 'application/json',
                        "responseText": 'Hello from the world'
                    });

                    spyOn(service, 'ping').and.callThrough();
                    spyOn(viewModel, 'addAgentToList');

                    //Act
                    let result = viewModel.addAgent(agent);

                    //Assert
                    expect(service.ping).toHaveBeenCalled();
                });
            });

            describe("When agent is reachable and healthy", () => {
                describe("When agent is changed details and healthy", () => {
                    it("Should save agent new details using storage service", () => {
                        //Arrange
                        let storage = new storageService();
                        let service = new agentService(storage);
                        let viewModel = new updateViewModel();
                        let agent = new agentTestBuilder()
                            .withName("Agent 2")
                            .withIpAddress("192.168.11.102")
                            .withPort(8281)
                            .build();

                        spyOn(viewModel, 'getAgentService').and.returnValue(service);
                        spyOn(service, 'updateAgent').and.callThrough();

                        //Act
                        viewModel.updateAgent(agent);
                        //Assert
                        expect(storage.getAgents()).toContain(agent);
                        //assert
                        expect(viewModel.ShowAddAgentForm()).toBe(false)
                    });
                });
            });
        });
    });
});

//working on 
describe("ValidateAgentName", function () {
    describe("If Agent name exists", function () {
        it("Should display error message", function () {
            //Arrange 
            let viewModel = new addViewModel();

            //Act
            result = viewModel.ValidateAgentName(agent.name);

            //Assert
            expect(result).toBe("Error_Message")
        });
    });
});
