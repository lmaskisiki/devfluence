using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace TerminatorWebApi.Tests
{
    [TestFixture]
    public class ApiTests
    {
        [Test]
        public void Health_WhenCalled_ShouldReturnStatusCodeOK()
        {
            // Arrange
            var bootstraper= new DefaultNancyBootstrapper();
            var browser = new Browser(bootstraper);
 
            // Act
            var result = browser.Get("/health", with =>
            {
                with.HttpRequest();
            });

            // Assert
         ///    Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var content = result.ContentType;
        }

        // var browser = new Browser(with => { with.Module<TerminatorApiModule>(); });

    }
}
