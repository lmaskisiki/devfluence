describe("ViewModel", () => {
    describe("runCommand", () => {

        beforeEach(() => {
            jasmine.Ajax.install();
        });
        afterEach(() => {
            jasmine.Ajax.uninstall();
        });

        describe("When getting IP Address", () => {
            it("Should call agentService runCommand method", () => {
                let service = new agentService();
                let viewModel = new addViewModel(service)

                let agent = new agentTestBuilder()
                    .withName("Agent 1")
                    .withIpAddress("localhost")
                    .withPort(1234)
                    .build();

                spyOn(service, 'runCommand').and.callThrough();

                //Act
                viewModel.runCommand("ip", agent);

                //Assert
                expect(service.runCommand).toHaveBeenCalledWith("ip", agent);
            });
        })

        describe("When getting IP Address, Given agent is health", () => {
            it("Should print output", () => {
                let service = new agentService();
                let viewModel = new addViewModel(service)

                let fn = jasmine.createSpy('spyName');

                let agent = new agentTestBuilder()
                    .withName("Agent 1")
                    .withIpAddress("localhost")
                    .withPort(1234)
                    .build();

                spyOn(viewModel, 'runCommand').and.callThrough();
                spyOn(service, 'runCommand').and.callThrough();

                //Act
                viewModel.runCommand("ip", agent, fn);

                //Assert
                expect(service.runCommand).toHaveBeenCalledWith("ip", agent);
                expect(fn).toHaveBeenCalled();
            });
        })
    })
});


describe("addViewModel", function () {
    describe("ExecuteCommand", function () {

        describe("when getting script", () => {
            beforeEach(() => {
                jasmine.Ajax.install();
            });
            afterEach(() => {
                jasmine.Ajax.uninstall();
            });
            it("Should execute command", () => {
                //Arrange
                let service = new agentService();
                let viewModel = new addViewModel(service);
                let agent = new agentTestBuilder()
                    .withName("Agent 1")
                    .withIpAddress("localhost")
                    .withPort(1234)
                    .build();

                var doneFn = jasmine.createSpy('success');

                // jasmine.Ajax.stubRequest('http://localhost:1234/api/ip').andReturn({
                //     "status": 200,
                //     "contentType": 'application/json',
                //     "responseText": '172.12.0.6'
                // });
                spyOn(service, 'runCommand').and.callThrough();

                //Act
                service.runCommand("ip", agent, doneFn);
                //Assert
            });
        });

    });
});