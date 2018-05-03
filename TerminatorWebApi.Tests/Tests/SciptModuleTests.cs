using System;
using System.IO;
using System.Reflection;
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

            var filePath = GetScriptPath("helloWorldScript");
            scriptExecutor.ExecutePowershell(filePath).Returns(new ScriptOutput { Message = "Hello World", StatusCode = 0 });
           
            //Act
            var result = browser.Post("/api/script", with =>
            {
                with.Header("Accept", "application/json");
                with.Body(filePath);
                with.HttpRequest();
            });

<<<<<<< HEAD
            //Assert  
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            // Assert.AreEqual(ScriptOutput.Message ,  )
=======
            //Assert 
            var expectedScriptResult = "Hello World";
            var actual = JsonConvert.DeserializeObject(result.Body.AsString());
            Assert.AreEqual(expectedScriptResult,actual);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);  
>>>>>>> 411c370a4fa971e5cb0d9ecbe1a929699e1f7197
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

            var filePath = GetScriptPath("invalidScript1");
            scriptExecutor.ExecutePowershell(filePath).Returns(new ScriptOutput { Message = "Invalid powershell script", StatusCode = 1 });

            //Act
            var result = browser.Post("/api/script", with =>
            {
                with.Header("Accept", "application/json");
                with.Body(filePath);
                with.HttpRequest();
            });

            //Assert  
            var expected = "Invalid powershell script";
            var actual = JsonConvert.DeserializeObject(result.Body.AsString());
            Assert.AreEqual(expected, actual);
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
                with.ApplicationStartupTask<ApplicationStartup>();
            });

            var filePath = GetScriptPath("helloWorldScript");
            scriptExecutor.ExecutePowershell(filePath).Throws(new Exception("Something went wrong"));

            //Act
            var result = browser.Post("/api/script", with =>
            {
                with.Header("Accept", "application/json");
                with.Body(filePath);
                with.HttpRequest();
            });

            //Assert  
<<<<<<< HEAD

=======
            var expected = "Something went wrong";
            var actual = JsonConvert.DeserializeObject(result.Body.AsString());
            Assert.AreEqual(expected,actual);
>>>>>>> 411c370a4fa971e5cb0d9ecbe1a929699e1f7197
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("Somthing went wrong", result.Body.AsString());
        }

        [Test]
        public void ExecuteScript_GivenEmptyScript_ShouldReturnStatusCode400()
        {
            //Arrange
            var scriptExecutor = Substitute.For<IScriptExecutor>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IScriptExecutor>(scriptExecutor);
                with.Module<ScriptExecutorModule>();
            });

            var filePath = GetScriptPath("emptyScript");
            
            //Act
            var result = browser.Post("/api/script", with =>
            {
                with.Header("Accept", "application/json");
                with.Body(filePath);
                with.HttpRequest();
            });

            //Assert  
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        private string GetScriptPath(string fileName)
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var iconPath = Path.Combine(outPutDirectory, "..", "..", $@"scripts\{fileName}.ps1");
            return new Uri(iconPath).LocalPath;
        }
    }
}
