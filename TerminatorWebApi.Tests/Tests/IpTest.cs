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
    public class IpTest
    {
        [TestCase("192.168.140.1")]
        [TestCase("192.168.140.2")]
        [TestCase("192.168.140.3")]
        public void GetIpadddress_WhenRequested_ShouldReturnStatusCode200AndIPAddress(string hostIpAddress)
        {
            // ---- Arrange ----
            var ipAddressGenerator = Substitute.For<IIpAddressGenerator>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IIpAddressGenerator>(ipAddressGenerator);
                with.Module<IpendpointModule>();
            });

            ipAddressGenerator.GetIpAddress().Returns(new ExecutionOutput { Output = hostIpAddress });

            // ---- Act ----
            var result = browser.Get("/api/ip", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
            });

            // ---- Assert ----   
            var actualResponseBody = GetResponseBody(result);
            ipAddressGenerator.Received(1).GetIpAddress();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(hostIpAddress, actualResponseBody.Output);
        }
        
        [Test]
        public void GetIpAddress_WhenExecutionFails_ShouldReturnStatusError500()
        {
            // ---- Arrange ----
            var ipAddressGenerator = Substitute.For<IIpAddressGenerator>();
            ipAddressGenerator.GetIpAddress().Throws(new Exception("invalid execution"));

            var browser = new Browser(with =>
            {
                with.Dependencies<IIpAddressGenerator>(ipAddressGenerator);
                with.Module<IpendpointModule>();
            });

            // ---- Act ----
            var result = browser.Get("/api/ip", with =>
            {
                with.HttpRequest();
            });

            // ---- Assert ----
            ipAddressGenerator.Received(1).GetIpAddress();
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        private static ExecutionOutput GetResponseBody(BrowserResponse result)
        {
            return JsonConvert.DeserializeObject<ExecutionOutput>(result.Body.AsString());
        }
    }
}
