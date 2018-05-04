using System;
using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TerminatorWebApi.Tests
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

            scriptExecutor.ExecutePowershell(scriptText).Returns(new ScriptOutput { Message = expectedOutput, StatusCode = 0 });

            //Act
            var result = browser.Post("/api/script", with =>
            {
                with.Header("Accept", "application/json");
                with.JsonBody(new ScriptQuery { Script = scriptText });
                with.HttpRequest();
            });

            //Assert 
            scriptExecutor.Received(1).ExecutePowershell(scriptText);
            var actual = JsonConvert.DeserializeObject<ScriptOutput>(result.Body.AsString());
            Assert.AreEqual(expectedOutput, actual.Message);
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
                with.JsonBody(new ScriptQuery { Script = scriptText });
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
                with.JsonBody(new { Text = scriptText });
                with.HttpRequest();
            });

            //Assert  
            scriptExecutor.DidNotReceiveWithAnyArgs().ExecutePowershell(Arg.Any<string>());
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
