using FluentAssertions;
using Nancy.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TerminatorWebApi.Modules;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Dapper;
using MachineInformationApp.Interfaces;
using MachineInformationApp;

namespace TerminatorWebApi.Tests.Tests
{
    [TestFixture]
    public class ExecutionAgentTests
    {
        [Test]
        public void GetExecutionAgentDetails_WhenExecuted_ShouldReturnAgentExecutionResult()
        {
            //Arrange
            var executionAgent = new AgentDataService("SkyNetPortalDB");
 
            //Act
            var actual = executionAgent.GetExecutedAgentDetails();

            var actualFirst = new List<AgentExecution>
            {
                actual.First()
            };

            //Assert
            var expectedFirst = new List<AgentExecution>
            {
                new AgentExecution
                {
                    ExecutionId = 1,
                    Command="ip",
                    Result="193.192.2.194",
                    ExecutionTime=DateTime.Parse("2018-12-31 23:59:59")
                }

            };
            actualFirst.Should().BeEquivalentTo(expectedFirst);
        }

        [Test]
        public void GetExecutionListAgentsDetails_WhenExecuted_ShouldReturnAgentsExecutionResult()
        {
            //Arrange
            var executionAgent = new AgentDataService("SkyNetPortalDB");

            //Act
            var actual = executionAgent.GetExecutedListAgentDetails();

            var actualFirstAndLast = new List<AgentExecution>
            {
                actual.First(),
                actual.Last()
            };

            //Assert
            var expectedFirst = new List<AgentExecution>
            {
                new AgentExecution
                {
                    ExecutionId =1,
                    Command="ip",
                    Result="193.192.2.194",
                    ExecutionTime=DateTime.Parse("2018-12-31 23:59:59")
                },
                 new AgentExecution
                {
                    ExecutionId = 2,
                    Command="hostname",
                    Result="DevFluence7",
                    ExecutionTime=DateTime.Parse("2018-01-01 21:59:59")
                }
            };
            actualFirstAndLast.Should().BeEquivalentTo(expectedFirst);
        }

        [Test]
        public void GetExecutionListAgentsDetails_WhenExecuted_ShouldReturnAllAgentsExecutionResult()
        {
            //Arrange
            var executionAgent = new AgentDataService("SkyNetPortalDB");

            //Act
            var agents = executionAgent.GetExecutedListAgentDetails();

            //Assert
            Assert.AreEqual(2, agents.Count());
        }

    }
}
