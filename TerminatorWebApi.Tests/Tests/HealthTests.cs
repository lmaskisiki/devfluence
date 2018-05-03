using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace TerminatorWebApi.Tests
{
    [TestFixture]
     class HealthTests
    {
        [Test]
        public void GetHealth_WhenCalled_ShouldReturnStatusCodeOK()
        {
            // ---- Arrange ---- 
            var browser = new Browser(with => with.Module(new HealthModule()));

            // ---- Act ----
            var result = browser.Get("/api/health", with => { with.HttpRequest(); });
            
            // ---- Assert ----       
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
