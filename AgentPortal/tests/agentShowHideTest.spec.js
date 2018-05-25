fdescribe("ShowAddAgentForm", function () {
    describe("If show add agent is set to false", function () {
        it("Should hide add-agent form", function () {
            //Arrange
            let viewModel = new addViewModel();

            //act
            viewModel.ShowAddAgentForm(false);

            //Assert
            expect(viewModel.ShowAddAgent()).toBe(false);
        });
    });
    describe("If set to true", function () {
        it("should show agent form", function () {
            //Arrange
            let viewModel = new addViewModel();

            //Act
            viewModel.ShowAddAgentForm(true);

            //Assert
            expect(viewModel.ShowAddAgent()).toBe(true);
        });
    });
});