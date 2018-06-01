describe("DashboadActivity", () => {
    beforeEach(() => {
        jasmine.Ajax.install();
    });
    afterEach(() => {
        jasmine.Ajax.uninstall();
    });
     describe("Add Agent", () => {
        it("Should save the activity", () => {
            //Arrange
            let service = new agentService();
            let viewModel = new addViewModel(service);
            let agent = new agentTestBuilder()
                .withName("Agent 1")
                .withIpAddress("192.168.11.101")
                .withPort(8282)
                .build();

            jasmine.Ajax.stubRequest(`http://192.168.11.101:8282/api/health`).andReturn({
                "status": 200,
                "contentType": 'application/json',
                "responseText": ""
            });

            jasmine.Ajax.stubRequest(`http://${agent.ipAddress}:${agent.port}/api/dashboardActivity}`).andReturn({
                "status": 200,
                "contentType": 'application/json',
                "responseText": ""
            });

            spyOn(service, 'ping').and.callThrough();
            spyOn(service, 'activityLogger');

            //Act
            let result = viewModel.addAgent(agent);

            //Assert
            expect(service.ping).toHaveBeenCalled();
            expect(service.activityLogger).toHaveBeenCalledWith("Dashboard", "Add Agent", "Agent 1 added");
        });
    });
    describe("Add Agent", () => {
        it("Should save the activity", () => {
            //Arrange
            let service = new agentService();
            let viewModel = new addViewModel(service);
            let agent = new agentTestBuilder()
                .withName("Agent 1")
                .withIpAddress("192.168.11.101")
                .withPort(8282)
                .build();

            jasmine.Ajax.stubRequest(`http://192.168.11.101:8282/api/health`).andReturn({
                "status": 400,
                "contentType": 'application/json',
                "responseText": ""
            });

            jasmine.Ajax.stubRequest(`http://${agent.ipAddress}:${agent.port}/api/dashboardActivity}`).andReturn({
                "status": 200,
                "contentType": 'application/json',
                "responseText": ""
            });

            spyOn(service, 'ping').and.callThrough();
            spyOn(service, 'activityLogger');

            //Act
            let result = viewModel.addAgent(agent);

            //Assert
            expect(service.ping).toHaveBeenCalled();
            expect(service.activityLogger).toHaveBeenCalledWith("Dashboard", "Add Agent", "Could Contact Agent 1");
        });
    });

    describe("Remove Agent", () => {
        it("Should save the activity", () => {
            //Arrange
            let service = new agentService();
            let viewModel = new addViewModel(service);
            let agent = new agentTestBuilder()
                .withName("Agent 1")
                .withIpAddress("192.168.11.101")
                .withPort(8282)
                .build();

            jasmine.Ajax.stubRequest(`http://${agent.ipAddress}:${agent.port}/api/dashboardActivity}`).andReturn({
                "status": 200,
                "contentType": 'application/json',
                "responseText": ""
            });

            spyOn(service, 'activityLogger');

            //Act
            viewModel.agents()[0] = agent;
            viewModel.activeAgent(agent);

            //Act
            viewModel.removeAgent();
            //Assert

            expect(viewModel.agents()).not.toContain(agent);
            expect(service.activityLogger).toHaveBeenCalledWith("Dashboard", "Remove Agent", "Agent 1 Removed");

        });
    });
    describe("Execute script ", () => {
        fit("should save dashboard activity ", () => {
            //Arrange
            let service = new agentService();
            let viewModel = new addViewModel(service);
            let agent = new agentTestBuilder()
                .withName("Agent 1")
                .withIpAddress("192.168.11.101")
                .withPort(8282)
                .build();


            jasmine.Ajax.stubRequest(`http://192.168.11.101:8282/api/ip`).andReturn({
                "status": 200,
                "contentType": 'application/json',
                "responseText": '{"result":"192.168.11.101"}'
            });

            jasmine.Ajax.stubRequest(`http://${agent.ipAddress}:${agent.port}/api/dashboardActivity}`).andReturn({
                "status": 200,
                "contentType": 'application/json',
                "responseText": ""
            });

           
            spyOn(service, 'activityLogger');
            viewModel.agents()[0] = agent;
            viewModel.activeAgents()[0] = viewModel.agents()[0];
            viewModel.commandToRun("ip");
            viewModel.runCommand();
            //Act
            let result = viewModel.runCommand();

            //Assert
            expect(service.activityLogger).toHaveBeenCalledWith(agent.name, "ip", "192.168.11.101");

        });
    });
});