using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace TerminatorWebApi.Tests
{
    [TestFixture]
    class ApiTests
    {
        [Test]
        public void Health_WhenCalled_ShouldReturnStatusCodeOK()
        {
            // Arrange 
            var browser = new Browser(with => with.Module(new HealthApi()));

            // Act
            var result = browser.Get("/health");

            // Assert       
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
