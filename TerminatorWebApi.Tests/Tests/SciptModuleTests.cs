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

namespace TerminatorWebApi.Tests.Tests
{
    [TestFixture]
    public class SciptModuleTests
    {
        [TestCase("echo 'Hello World'", "Hello World")]
        [TestCase("Write-Output 'Hello Bob'", "Hello Bob")]
        public void ExecuteScript_GivenValidPowershellScript_ShouldReturnStatusCode200AndOutput(string scriptText, string expectedOutput)
        {
            //Arrange
            var scriptExecutor = Substitute.For<IScriptExecutor>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IScriptExecutor>(scriptExecutor);
                with.Module<ScriptExecutorModule>();
            });

            scriptExecutor.ExecutePowershell(scriptText).Returns(new ScriptOutput { Output = expectedOutput, ExitCode = 0 });

            //Act
            var result = browser.Post("/api/script", with =>
            {
                with.Header("Accept", "application/json");
                with.JsonBody(new ScriptQuery { PowerShellScript = scriptText });
                with.HttpRequest();
            });

            //Assert 
            scriptExecutor.Received(1).ExecutePowershell(scriptText);
            var actual = JsonConvert.DeserializeObject<ScriptOutput>(result.Body.AsString());
            Assert.AreEqual(expectedOutput, actual.Output);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestCase("echo 'Hello World'")]
        [TestCase("Write-Output 'Hello World'")]
        public void ExecuteScript_GivenValidPowershellScriptAndSomethingGoesWrong_ShouldReturnStatusCode500(string scriptText)
        {
            //Arrange
            var scriptExecutor = Substitute.For<IScriptExecutor>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IScriptExecutor>(scriptExecutor);
                with.Module<ScriptExecutorModule>();
            });

            scriptExecutor.ExecutePowershell(scriptText).Throws(new Exception("Something went wrong"));

            //Act
            var result = browser.Post("/api/script", with =>
            {
                with.Header("Accept", "application/json");
                with.JsonBody(new ScriptQuery { PowerShellScript = scriptText });
                with.HttpRequest();
            });

            //Assert  
            var expected = "Something went wrong";
            var actual = result.Body.AsString();
            scriptExecutor.Received(1).ExecutePowershell(scriptText);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase("\t")]
        public void ExecuteScript_GivenEmptyScript_ShouldReturnStatusCode400(string scriptText)
        {
            //Arrange
            var scriptExecutor = Substitute.For<IScriptExecutor>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IScriptExecutor>(scriptExecutor);
                with.Module<ScriptExecutorModule>();
            });

            //Act
            var result = browser.Post("/api/script", with =>
            {
                with.Header("Accept", "application/json");
                with.JsonBody(new { Script = scriptText });
                with.HttpRequest();
            });

            //Assert  
            scriptExecutor.DidNotReceiveWithAnyArgs().ExecutePowershell(Arg.Any<string>());
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void ExecuteScript_GivenInvalidPowershellCommand_ShouldReturnStatusCode200AndPowershellErrorMessage()
        {
            //Arrange
            var scriptExecutor = Substitute.For<IScriptExecutor>();
            var expected = "The term 'Sindy' is not recognized as the name of a cmdlet, function, script file, or operable program. Check the spelling of the name, or if a path was included, verify that the path is correct and try again";
            var scriptText = "Sindy";
            var browser = new Browser(with =>
            {
                with.Dependencies<IScriptExecutor>(scriptExecutor);
                with.Module<ScriptExecutorModule>();
            });

            scriptExecutor.ExecutePowershell(scriptText).Returns(new ScriptOutput { Output = expected, ExitCode = 1 });
            //Act
            var result = browser.Post("/api/script", with =>
            {
                with.Header("Accept", "application/json");
                with.JsonBody(new { PowerShellScript = scriptText });
                with.HttpRequest();
            });

            //Assert  
            var executionOutput = GetResponseBody(result);
            scriptExecutor.Received().ExecutePowershell(Arg.Any<string>());
            Assert.AreEqual(expected, executionOutput.Output);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        [Test]
        public void ExecuteScript_GivenInvalidPowershellCommand_ShouldReturnStatusCode200AndSystemPowershellErrorMessage()
        {
            //Arrange
            var scriptExecutor = Substitute.For<IScriptExecutor>();
            var scriptText = "'hello' World";
            var expected = "At line:1 char:9\r\n + 'hello' World\r\n + ~~~~~\nUnexpected token 'World' in expression or statement";
            var browser = new Browser(with =>
            {
                with.Dependencies<IScriptExecutor>(scriptExecutor);
                with.Module<ScriptExecutorModule>();
            });
            scriptExecutor.ExecutePowershell(scriptText).Returns(new ScriptOutput { Output = expected, ExitCode = 1 });

            //Act
            var result = browser.Post("/api/script", with =>
            {
                with.Header("Accept", "application/json");
                with.JsonBody(new { PowerShellScript = scriptText });
                with.HttpRequest();
            });

            //Assert  
            var executionOutput = GetResponseBody(result);
            scriptExecutor.Received().ExecutePowershell(Arg.Any<string>());
            Assert.AreEqual(expected, executionOutput.Output);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }








        [Test, Ignore("check binding")]
        public void SaveExecution_GivenNullAgentExecution_ShouldNotSave()
        {
            //Arrange
            var agentDataService = Substitute.For<IAgentDataService>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IAgentDataService>(agentDataService);
                with.Module<AgentHistoryModule>();
            });

            //  AgentExecution agentExecution = null;
            //Act
            var result = browser.Post("/api/agentHistory", with =>
            {
                with.Header("Accept", "application/json");
                with.JsonBody("");
                with.HttpRequest();
            });

            //Assert  
            agentDataService.DidNotReceiveWithAnyArgs().GetInsertedData(Arg.Any<AgentExecution>());
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }


        [Test]
        public void SaveExecution_GivenValidAgentExecution_ShouldSaveAndReturn1()
        {
            //Arrange
            var agentDataService = Substitute.For<IAgentDataService>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IAgentDataService>(agentDataService);
                with.Module<AgentHistoryModule>();
            });

            agentDataService.GetInsertedData(Arg.Any<AgentExecution>()).Returns(1);

            //Act
            var result = browser.Post("/api/agentHistory", with =>
            {
                with.Header("Accept", "application/json");
                with.JsonBody(new AgentExecution { Command = "Ip", Result = "", ExecutionTime = DateTime.UtcNow });
                with.HttpRequest();
            });

            //Assert  
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            agentDataService.Received(1).GetInsertedData(Arg.Any<AgentExecution>());
        }


        [Test]
        public void GetExecutions_ShouldReturnStatusCode200AndExisitingExecution()
        {
            //Arrange
            var agentDataService = Substitute.For<IAgentDataService>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IAgentDataService>(agentDataService);
                with.Module<AgentHistoryModule>();
            });

            var agentExecutions = new List<AgentExecution>()
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

            agentDataService.GetExecutedListAgentDetails().Returns(agentExecutions);

            //Act
            var result = browser.Get("/api/agentHistory", with =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            //Assert  
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            IEnumerable<AgentExecution> executionsFound = JsonConvert.DeserializeObject<List<AgentExecution>>(result.Body.AsString());
            agentDataService.Received(1).GetExecutedListAgentDetails();
            executionsFound.Should().BeEquivalentTo(agentExecutions);

        }

        private static ScriptOutput GetResponseBody(BrowserResponse result)
        {
            return JsonConvert.DeserializeObject<ScriptOutput>(result.Body.AsString());
        }
    }


}
