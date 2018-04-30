using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        [Test]
        public void ExecuteScript_GivenValidPowershellScript_ShouldReturnStatusCodeOK()
        {
            //Arrange
            var scriptExecutor = Substitute.For<IScriptExecutor>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IScriptExecutor>(scriptExecutor);
                with.Module<ScriptExecutorModule>();
            });

            var filePath = @"C:\Dev\TerminatorApp\MachineInformationApp\MachineinformationApp.Tests\scripts\helloWorldScript.ps1";
            scriptExecutor.ExecutePowershell(filePath).Returns(new ScriptOutput { Message = "Hello World", StatusCode = 0 });

            //Act
            var result = browser.Post("/script", with =>
            {
                with.Header("Accept", "application/json");
                with.Body(filePath);
                with.HttpRequest();
            });

            //Assert  
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void ExecuteScript_GivenInvalidPowershellScript_ShouldReturnStatusCode500()
        {
            //Arrange
            var scriptExecutor = Substitute.For<IScriptExecutor>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IScriptExecutor>(scriptExecutor);
                with.Module<ScriptExecutorModule>();
            });

            var filePath = @"C:\Dev\TerminatorApp\MachineInformationApp\MachineinformationApp.Tests\scripts\invalidScript1.ps1";
            scriptExecutor.ExecutePowershell(filePath).Returns(new ScriptOutput { Message = "", StatusCode = 1 });

            //Act
            var result = browser.Post("/script", with =>
            {
                with.Header("Accept", "application/json");
                with.Body(filePath);
                with.HttpRequest();
            });

            //Assert  
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Test]
        public void ExecuteScript_GivenValidPowershellScriptAndSomethingGoesWrong_ShouldReturnStatusCode500()
        {
            //Arrange
            var scriptExecutor = Substitute.For<IScriptExecutor>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IScriptExecutor>(scriptExecutor);
                with.Module<ScriptExecutorModule>();
            });

            var filePath = @"C:\Dev\TerminatorApp\MachineInformationApp\MachineinformationApp.Tests\scripts\helloWorldScript.ps1";
            scriptExecutor.ExecutePowershell(filePath).Throws(new Exception("Somthing went wrong"));

            //Act
            var result = browser.Post("/script", with =>
            {
                with.Header("Accept", "application/json");
                with.Body(filePath);
                with.HttpRequest();
            });

            //Assert  
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
    }
}
