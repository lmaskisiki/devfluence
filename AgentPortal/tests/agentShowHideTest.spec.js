describe("ShowAddAgentForm", function () {
    describe("If set to false", function () {
        xit("Should hide add agent form", function () {
            //Arrange
            let viewModel = new addViewModel();

            //act
            viewModel.ShowAddAgentForm(false);

            //Act
            viewModel.addAgent(agent);
            //Assert
            expect(storage.getAgents()).toContain(agent);
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