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
using NUnit.Framework.Internal;

namespace TerminatorWebApi.Tests
{
    [TestFixture]
    public class OSTest
    {
        [Test]
        public void GetOperatingSystem_WhenRequested_ShouldReturnOS()
        {
            //Arrange
            var osGenerator = Substitute.For<IOSGenerator>();
            new OSEndpointModule(osGenerator);
            var browser = new Browser(with =>
            {
                with.Dependencies<IOSGenerator>(osGenerator);
                with.Module<OSEndpointModule>();
            });
            osGenerator.GetOsVersion().Returns("Microsoft Windows NT 10.0.16299.0");

            //Act
            var result = browser.Get("/os", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
            });
            //Assert  
            var expected = "Microsoft Windows NT 10.0.16299.0";
            Assert.AreEqual(expected,JsonConvert.DeserializeObject(result.Body.AsString()));
        }

    }
}
