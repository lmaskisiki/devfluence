using MachineInformationApp;
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
    public class OSTest
    {
        [Test]
        public void GetOperatingSystem_WhenRequested_ShouldReturnOS()
        {
            // ---- Arrange ----
            var osGenerator = Substitute.For<IOSGenerator>();

            var browser = new Browser(with =>
            {
                with.Dependencies<IOSGenerator>(osGenerator);
                with.Module<OSEndpointModule>();
            });
            osGenerator.GetOsVersion().Returns(new ExecutionOutput { Output = "Microsoft Windows NT 10.0.16299.0" });

            // ---- Act ----
            var result = browser.Get("/api/os", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
            });
            // ---- Assert ----
            osGenerator.Received(1).GetOsVersion();
            var expected = "Microsoft Windows NT 10.0.16299.0";
            var actualResponseBody = JsonConvert.DeserializeObject<ExecutionOutput>(result.Body.AsString());
            Assert.AreEqual(expected, actualResponseBody.Output);
        }

        [Test]
        public void GetOperatingSystem_WhenExecutionFails_ShouldReturnStatusCode500()
        {
            // ---- Arrange ----
            var osGenerator = Substitute.For<IOSGenerator>();
            osGenerator.GetOsVersion().Throws(new Exception("Execution failed"));

            var browser = new Browser(with =>
            {
                with.Dependencies<IOSGenerator>(osGenerator);
                with.Module<OSEndpointModule>();
            });

            // ---- Act ----
            var result = browser.Get("/api/os", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
            });

            // ---- Assert ----
            osGenerator.Received(1).GetOsVersion();
            var expected = "Execution failed";
            Assert.AreEqual(expected, result.Body.AsString());
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
    }
}
