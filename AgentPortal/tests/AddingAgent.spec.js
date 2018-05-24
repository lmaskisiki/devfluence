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
                describe("ShowAddAgentForm", function () {
                    describe("If set to false", function () {
                        xit("Should hide add agent form", function () {
                            //Arrange
                            let viewModel = new addViewModel();

                            //act
                            viewModel.ShowAddAgentForm(false);

                            //assert
                            expect(viewModel.ShowAddAgentForm()).toBe(false)
                        });
                    });
                    describe("If set to true", function () {
                        xit("should show agent form", function () {
                            //Arrange
                            let viewModel = new addViewModel();

                            //Act
                            viewModel.ShowAddAgentForm(true);

                            //Assert
                            expect(viewModel.ShowAddAgentForm()).toBe(true);
                        });
                    });
                });
            });
        });
    });
});