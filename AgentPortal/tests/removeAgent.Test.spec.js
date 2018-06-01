describe("removeAgent", () => {
    beforeEach(() => {
        jasmine.Ajax.install();
    });
    afterEach(() => {
        jasmine.Ajax.uninstall();
    });
    describe("When agent is removed", () => {
        fit("Should not exist in the list of available agents", () => {
            //Arrange
            let service = new agentService();
            let viewModel = new addViewModel(service);

            let agent = new agentTestBuilder()
                .withName("Agent 1")
                .withIpAddress("192.168.11.101")
                .withPort(8282)
                .build();


            viewModel.agents()[0] = agent;
            viewModel.activeAgent(agent);

            //Act
            viewModel.removeAgent();
            //Assert
             expect(viewModel.agents()).not.toContain(agent);
        });
    });
});
