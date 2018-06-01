describe("DashboadActivity", () => {
    beforeEach(() => {
        jasmine.Ajax.install();
    });
    afterEach(() => {
        jasmine.Ajax.uninstall();
    });
    describe("Add Agent", () => {
        fit("Should save the activity", () => {
            //Arrange
            let service = new agentService();
            let viewModel = new addViewModel(service);
            let agent = new agentTestBuilder()
                .withName("Agent 1")
                .withIpAddress("192.168.11.101")
                .withPort(8282)
                .build();

            jasmine.Ajax.stubRequest(`http://${agent.ipAddress}:${agent.port}/api/health}`).andReturn({
                "status": 200,
                "contentType": 'application/json',
                "responseText": ""
            });

            spyOn(service, 'ping');

            spyOn(viewModel, 'setAgentStatusTo');



            //Act
            let result = viewModel.addAgent(agent);

            //Assert
            expect(service.ping).toHaveBeenCalled();
         });
    });
});