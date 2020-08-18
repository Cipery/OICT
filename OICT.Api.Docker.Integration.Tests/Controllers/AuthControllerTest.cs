using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using OICT.Api.Docker.Integration.Tests.Utils;
using OICT.Tests.Common;
using Xunit;
using Xunit.Abstractions;

namespace OICT.Api.Docker.Integration.Tests.Controllers
{
    public class AuthControllerTest : IClassFixture<ApiWebAppFactory<Program>>
    {
        private readonly ApiWebAppFactory<Program> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public AuthControllerTest(ApiWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Should_ReturnJwt_WhenCorrectAuthCredentialsAreProvided()
        {
            // Arrange
            var payload = new { Username = "admin", Password = "123456" };
            using var client = _factory.CreateHttpClient();

            // Act
            var result = await client.PostAsync("/api/Auth/login", payload.AsJson());

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_WhenIncorrectAuthCredentialsAreProvided()
        {
            // Arrange
            var payload = new { Username = Id.StringValue1, Password = Id.StringValue2 };
            using var client = _factory.CreateHttpClient();

            // Act
            var result = await client.PostAsync("/api/Auth/login", payload.AsJson());

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
