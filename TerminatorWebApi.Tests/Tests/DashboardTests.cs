using System;
using System.Collections.Generic;
using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TerminatorWebApi.Modules;
using FluentAssertions;
using System.Linq;

namespace TerminatorWebApi.Tests.Tests
{
    [TestFixture]
    public class DashboardTests
    {

        //-----------------Test For Database------------------
        [Test]
        public void GetDashboardDetails_WhenExecuted_ShouldReturnDashBoardDetails()
        {
            //Arrange
            var execution = new DashboardDataService("SkyNetPortalDB");

            //Act 
            var actual = execution.GetDashboardDetails();

            var fisrtRecordInDB = new List<DashboardExecution>
            {
                actual.First()
            };

            //Assert 
            var expectFirstInDB = new List<DashboardExecution>
            {
                new DashboardExecution
                {        
                    ID = 1,
                    Target = "Dashboard",
                    Action = "Add Agent",
                    ActionResult = "Added Agent 1",
                    ExecutionTime = DateTime.Parse("2018-12-31 23:59:59")
                }

            };
            fisrtRecordInDB.Should().BeEquivalentTo(expectFirstInDB);
        }

       
        //-----------------Test For Dash board ------------------

        [Test]
        public void InsertDetails_GivenValidAgentExecution_ShouldSaveAndReturnExecutionAgentOfAnId1()
        {
            //Arrange
            var dashBoardDataService = Substitute.For<IDashboardDataService>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IDashboardDataService>(dashBoardDataService);
                with.Module<DashboardHistoryModule>();
            });

            dashBoardDataService.GetInsertDetails(Arg.Any<DashboardExecution>()).Returns(1);

            //Act
            var result = browser.Post("/api/dashboardActivity", with =>
            {
                with.Header("Accept", "application/json");
                with.JsonBody(new DashboardExecution { Target = "Agent 1", Action = "Execute Command",ActionResult="IP Address", ExecutionTime = DateTime.UtcNow });
                with.HttpRequest();
            });

            //Assert  
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            dashBoardDataService.Received(1).GetInsertDetails(Arg.Any<DashboardExecution>());
        }

        [Test]
        public void GetDashboardExecutions_ShouldReturnStatusCode200AndExisitingExecution()
        {
            //Arrange
            var dashBoardDataService = Substitute.For<IDashboardDataService>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IDashboardDataService>(dashBoardDataService);
                with.Module<DashboardHistoryModule>();
            });

            var dashboardExecutions = new List<DashboardExecution>()
            {
              new DashboardExecution
                {
                    ID =1,
                    Target = "Dashboard",
                    Action="Add Agent",
                    ActionResult="Added Agent Sachin",
                    ExecutionTime=DateTime.UtcNow
                },
                 new DashboardExecution
                {
                    ID =1,
                    Target = "Agent Sachin",
                    Action="OS Name",
                    ActionResult="Windows",
                    ExecutionTime=DateTime.UtcNow
                }
            };

            dashBoardDataService.GetDashboardDetails().Returns(dashboardExecutions);

            //Act
            var result = browser.Get("/api/dashboardActivity", with =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            //Assert  
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            IEnumerable<DashboardExecution> executionsFound = JsonConvert.DeserializeObject<List<DashboardExecution>>(result.Body.AsString());
            dashBoardDataService.Received(1).GetDashboardDetails();
            executionsFound.Should().BeEquivalentTo(dashboardExecutions);
        }
    }
}

