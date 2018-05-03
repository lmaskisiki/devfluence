using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using MachineInformationApp;

namespace TerminatorWebApi.Tests
{
    [TestFixture]
    public class HostnameTests
    {
        [Test]
        public void GetHostname_WhenCalled_ShouldReturnStatusCodeOk()
        {
            // ---- Arrange ----

            var hostnameGenerator = Substitute.For<IHostnameGenerator>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IHostnameGenerator>(hostnameGenerator);
                with.Module<HostnameModule>();
            });

            // ---- Act ----
            var response = browser.Get("/api/hostname", with =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            // ---- Assert ----
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestCase("Devfluence-10")]
        [TestCase("Devfluence-11")]
        [TestCase("Devfluence-12")]
        public void GetHostname_WhenCalled_ShouldReturnStatusMachineHostName(string givenHostName)
        {
            // ---- Arrange ----
            var hostnameGenerator = Substitute.For<IHostnameGenerator>();
            var browser = new Browser(with =>
           {
               with.Dependencies<IHostnameGenerator>(hostnameGenerator);
               with.Module<HostnameModule>();
           });

            hostnameGenerator.GetHostName().Returns(new ExecutionOutput { Output = givenHostName });

            // ---- Act ----
            var response = browser.Get("/api/hostname", with =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            // ---- Assert ----
            var expectedResponseBody = givenHostName;
            var actualResponseBody = JsonConvert.DeserializeObject<ExecutionOutput>(response.Body.AsString());
            Assert.AreEqual(expectedResponseBody, actualResponseBody.Output);
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
            Assert.AreEqual(expected, result.Body.AsString());
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Test]
        public void GetFullQualifiedHostName_WhenCalled_ShouldReturnStatusCode200AndFullyQualifiedHostName()
        {
            // ---- Arrange ----
            var fullyQualified = "Devfluence - 10-PC01";
            var hostnameGenerator = Substitute.For<IHostnameGenerator>();

            var browser = new Browser(with =>
            {
                with.Dependencies<IHostnameGenerator>(hostnameGenerator);

                with.Module<HostnameModule>();
            });
            hostnameGenerator.GetFullQualifiedHostName().Returns(new ExecutionOutput { Output = fullyQualified });

            // ---- Act ----
            var response = browser.Get("/api/hostname", with =>
            {
                with.Query("fully-qualified", "true");
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            // ---- Assert ----
            var expectedResponseBody = fullyQualified;
            var actualResponseBody = JsonConvert.DeserializeObject<ExecutionOutput>(response.Body.AsString());
            Assert.AreEqual(expectedResponseBody, actualResponseBody.Output);
        }
    }
}
