describe("removeViewModel", () => {
    describe("removeAgent", () => {
        describe("When agent is no longer needed", () => {
            xit("Should remove agent to using storage service", () => {
                //Arrange
                let service = new agentService();
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