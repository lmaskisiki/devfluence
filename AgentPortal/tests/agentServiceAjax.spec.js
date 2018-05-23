'use strict';
describe("agentService", () => {
    describe("doPost", () => {
        beforeEach(function () {
            jasmine.Ajax.install();
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });
        xit("Should post", () => {
            //Arrange

            let storage = new storageService();
            let service = new agentService(storage);

            let agent = new agentTestBuilder()
                .withName("lizo")
                .withIpAddress("ss")
                .withPort(2)
                .build();

            spyOn(service, 'addAgent').and.callThrough();

            //Act
            const result = viewModel.addAgent(agent);

            //Assert
            let request = jasmine.Ajax.requests.mostRecent()
            // expect(request.url).toMatch("/api/health");
            // expect(request.method).toBe('GET')
            // expect(request.data()).toEqual({});


        });
    });
})