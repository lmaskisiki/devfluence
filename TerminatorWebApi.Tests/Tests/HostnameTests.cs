using System;
using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TerminatorWebApi.Modules;

namespace TerminatorWebApi.Tests.Tests
{
    [TestFixture]
    public class HostnameTests
    {
        [TestCase("Devfluence-10")]
        [TestCase("Devfluence-11")]
        [TestCase("Devfluence-12")]
        public void GetHostname_WhenCalled_ShouldReturnStatusCode200AndMachineHostName(string givenHostName)
        {
            // ---- Arrange ----
            var hostnameGenerator = Substitute.For<IHostnameGenerator>();
            var browser = new Browser(with =>
           {
               with.Dependencies<IHostnameGenerator>(hostnameGenerator);
               with.Module<HostnameModule>();
           });

            hostnameGenerator.GetHostName().Returns(new ExecutionOutput { result = givenHostName });

            // ---- Act ----
            var result = browser.Get("/api/hostname", with =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            // ---- Assert ----
            var expectedResponseBody = givenHostName;
            var actualResponseBody = GetResponseBody(result);
            hostnameGenerator.Received(1).GetHostName();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedResponseBody, actualResponseBody.result);
        }

        [Test]
        public void GetHostName_WhenExecutionFails_ShouldReturnStatusCode500()
        {
            // ---- Arrange ----
            var hostnameGenerator = Substitute.For<IHostnameGenerator>();
            hostnameGenerator.GetHostName().Throws(new Exception("invalid execution"));

            var browser = new Browser(with =>
            {
                with.Dependency<IHostnameGenerator>(hostnameGenerator);
                with.Module<HostnameModule>();

            });

            // ---- Act ----
            var result = browser.Get("/api/hostname", with =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            // ---- Assert ----
            var expected = "invalid execution";
            hostnameGenerator.Received(1).GetHostName();
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(expected, result.Body.AsString());
        }

        [TestCase("Devfluence-10")]
        [TestCase("Devfluence-11")]
        [TestCase("Devfluence-12")]
        public void GetFullyQualifiedHostName_WhenCalled_ShouldReturnStatusCode200AndFullyQualifiedHostName(string machineFullyQualifiedHostName)
        {
            // ---- Arrange ----
            var hostnameGenerator = Substitute.For<IHostnameGenerator>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IHostnameGenerator>(hostnameGenerator);

                with.Module<HostnameModule>();
            });
            hostnameGenerator.GetFullQualifiedHostName().Returns(new ExecutionOutput { result = machineFullyQualifiedHostName });

            // ---- Act ----
            var result = browser.Get("/api/hostname", with =>
            {
                with.Query("fully-qualified", "true");
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            // ---- Assert ----
            var expectedResponseBody = machineFullyQualifiedHostName;
            var actualResponseBody = GetResponseBody(result);
            hostnameGenerator.Received(1).GetFullQualifiedHostName();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(expectedResponseBody, actualResponseBody.result);
        }

        private static ExecutionOutput GetResponseBody(BrowserResponse result)
        {
            return JsonConvert.DeserializeObject<ExecutionOutput>(result.Body.AsString());
        }
    }
}
