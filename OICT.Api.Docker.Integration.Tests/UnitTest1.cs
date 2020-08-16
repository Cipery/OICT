using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace OICT.Api.Docker.Integration.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _baseUrl;
        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _baseUrl = Environment.GetEnvironmentVariable("API_URL");
        }

        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }

        [Theory]
        [InlineData("/api/Employees")]
        public async Task WhenEndpointWithAuthRequiredIsCalled_WithoutBearerToken_401UnauthorizedIsReturned(string endpoint)
        {
            var client = new HttpClient();
            var result = await client.GetAsync(_baseUrl + endpoint);
            _testOutputHelper.WriteLine(_baseUrl + endpoint);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
    }
}
