using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;

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
            var response = browser.Get("/hostname", with =>
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

            hostnameGenerator.GetHostName().Returns(givenHostName);

            // ---- Act ----
            var response = browser.Get("/hostname", with =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            // ---- Assert ----
            var expectedResponseBody = givenHostName;
            var actualResponseBody = JsonConvert.DeserializeObject(response.Body.AsString());
            Assert.AreEqual(expectedResponseBody, actualResponseBody);
        }

        [Test]
        public void getHostName_WhenExecutionFails_ShouldReturnStatusCode500()
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
            var result = browser.Get("/hostname", with =>
            {
                with.HttpRequest();
            });

            // ---- Assert ----
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
    }
}
