using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace TerminatorWebApi.Tests
{
    [TestFixture]
    public class IpTest
    {
        [Test]
        public void GetIpadddress_WhenRequestingip_ShouldReturnStatuscode200()
        {
            //Arrange
            var ipAddressGenerator = Substitute.For<IIpAddressGenerator>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IIpAddressGenerator>(ipAddressGenerator);
                with.Module<IpendpointModule>();
            });

            //Act
            var result = browser.Get("/ip", with =>
            {
                with.Header("Accept", "application/json");
                with.HttpRequest();
            });

            //Assert  
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestCase("192.168.140.1")]
        [TestCase("192.168.140.2")]
        [TestCase("192.168.140.3")]
        public void GetIpadddress_WhenRequested_ShouldReturnIP(string hostIpAddress)
        {
            //Arrange
            var ipAddressGenerator = Substitute.For<IIpAddressGenerator>();
            var browser = new Browser(with =>
            {
                with.Dependencies<IIpAddressGenerator>(ipAddressGenerator);
                with.Module<IpendpointModule>();
            });

            ipAddressGenerator.GetIpAddress().Returns(hostIpAddress);

            //Act
            var result = browser.Get("/ip", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
            });

            //Assert  
            var expected = hostIpAddress;
            Assert.AreEqual(expected, JsonConvert.DeserializeObject(result.Body.AsString()));
        }
    }
}
