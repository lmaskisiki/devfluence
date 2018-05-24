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
                it("Should return error message 'NOT_FOUND'", () => {
                    //Arrange
                    let storage = new storageService();
                    let service = new agentService(storage);
                    let viewModel = new addViewModel();
                    let agent = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .withPort(8282)
                        .build();

                    spyOn(viewModel, 'getAgentService').and.returnValue(service);
                    spyOn(service, 'addAgent').and.returnValue('NOT_FOUND');

                    //Act
                    const result = viewModel.addAgent(agent);
                    //Assert
                    expect(service.addAgent).toHaveBeenCalledWith(agent);
                    expect(result).toBe("NOT_FOUND");
                });
            });

            describe("When agent is reachable and healthy", () => {
                it("Should save agent to using storage service", () => {
                    //Arrange
                    let storage = new storageService();
                    let service = new agentService(storage);
                    let viewModel = new addViewModel();
                    let agent = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .withPort(8282)
                        .build();

                    spyOn(viewModel, 'getAgentService').and.returnValue(service);
                    spyOn(service, 'addAgent').and.callThrough();

                    //Act
                    viewModel.addAgent(agent);
                    //Assert
                    expect(storage.getAgents()).toContain(agent);
                });
            });
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
                });
            });
        });
    });
            describe("removeViewModel", () => {
                describe("removeAgent", () => {
                    describe("When agent is no longer needed", () => {
                        it("Should remove agent to using storage service", () => {
                            //Arrange
                            let storage = new storageService();
                            let service = new agentService(storage);
                            let viewModel = new removeViewModel(service);
                           
                            spyOn(viewModel, 'removeAgent').and.returnValue(service);
                          
                            //Act
                            viewModel.removeAgent(agent);
                            //Assert
                            expect(storage.removeAgent()).not.toContain(agent);
                        });
                    });
                });
            });
        });
 