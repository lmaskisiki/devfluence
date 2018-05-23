

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
                    let viewModel = new addViewModel(service);
                    let agent = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .withPort(8282)
                        .build();

                    spyOn(service, 'ping').and.returnValue("NOT_FOUND");
                    spyOn(storage, 'addAgent');

                    //Act
                    const result = viewModel.addAgent(agent);
                    //Assert
                    expect(service.ping).toHaveBeenCalledWith(agent);
                    expect(storage.addAgent).not.toHaveBeenCalled();
                    expect(result).toBe("NOT_FOUND");
                });
            });

            describe("When agent is reachable and healthy", () => {
                it("Should save agent to using storage service", () => {
                    //Arrange
                    let storage = new storageService();
                    let service = new agentService(storage);
                    let viewModel = new addViewModel(service);
                    let agent = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .withPort(8282)
                        .build();

                    spyOn(service, 'ping').and.returnValue("SUCCESS")
                    spyOn(service, 'addAgent').and.callThrough();

                    //Act
                    let result = viewModel.addAgent(agent);
                    //Assert
                    expect(service.ping).toHaveBeenCalledWith(agent);
                    expect(service.addAgent).toHaveBeenCalledWith(agent);
                    expect(result).toBe("SUCCESS");
                });


                it("Should save agent to using storage service", () => {
                    //Arrange
                    let storage = new storageService();
                    let service = new agentService(storage);
                    let viewModel = new addViewModel(service);
                    let agent = new agentTestBuilder()
                        .withName("Agent 1")
                        .withIpAddress("192.168.11.101")
                        .withPort(8282)
                        .build();

                    
                });
            });
        });
    });
});