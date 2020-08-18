using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using OICT.Api.Docker.Integration.Tests.Utils;
using OICT.Application.Dtos;
using OICT.Tests.Common;
using Swashbuckle.AspNetCore.SwaggerUI;
using Xunit;
using Xunit.Abstractions;

namespace OICT.Api.Docker.Integration.Tests.Controllers
{
    /// <summary>
    /// So far, there are only happy path ("HDS" - Happy Day Scenario) tests
    /// </summary>
    public class EmployeeControllerTest : IClassFixture<ApiWebAppFactory<Program>>
    {
        private readonly ApiWebAppFactory<Program> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public EmployeeControllerTest(ApiWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
        }
        private async Task<int> CreateEmployee(HttpClient client, CreateEmployeeModel model)
        {
            var createResponse = await client.PostAsync(_createEmployeeEndpoint, model.AsJson());
            var content = await createResponse.Content.ReadAsStringAsync();
            var responseContentObj = JsonConvert.DeserializeAnonymousType(content, new { Id = 0 });
            return responseContentObj.Id;
        }

        #region Endpoint security

        public static IEnumerable<object[]> SecuredEndpoints =>
            new List<object[]>
            {
                new object[] {new HttpRequestMessage(HttpMethod.Get, "/api/Employees")},
                new object[] {new HttpRequestMessage(HttpMethod.Get, "/api/Employees/5")},
                new object[] {new HttpRequestMessage(HttpMethod.Get, "/api/Employees/listOlderThan/18")},
                new object[] {new HttpRequestMessage(HttpMethod.Put, "/api/Employees/5")},
                new object[] {new HttpRequestMessage(HttpMethod.Delete, "/api/Employees/5")},
                new object[] {new HttpRequestMessage(HttpMethod.Post, "/api/Employees")},
            };

        [Theory]
        [MemberData(nameof(SecuredEndpoints))]
        public async Task Should_Return401Unauthorized_WhenEndpointWithAuthRequiredIsCalledWithoutBearerToken(
            HttpRequestMessage requestMessage)
        {
            // Arrange
            using var client = _factory.CreateHttpClient();

            // Act
            var result = await client.SendAsync(requestMessage);

            // Assert
            result.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [MemberData(nameof(SecuredEndpoints))]
        public async Task Should_NotReturn401Unauthorized_WhenEndpointWithAuthRequiredIsCalledWithBearerToken(
            HttpRequestMessage requestMessage)
        {
            // Arrange
            using var client = await _factory.CreateAuthenticatedHttpClientAsync();

            // Act
            var result = await client.SendAsync(requestMessage);

            // Assert
            result.StatusCode.Should().NotBeEquivalentTo(HttpStatusCode.Unauthorized);
        }
        #endregion
        #region Create employee

        private readonly string _createEmployeeEndpoint = "/api/Employees";

        [Fact]
        public async Task Should_CreateAndReturnEmployee()
        {
            // Arrange
            using var client = await _factory.CreateAuthenticatedHttpClientAsync();

            // Act
            var createEmployeeObj = new CreateEmployeeModel
            {
                Name = Id.StringValue1,
                Active = true,
                ChildrenCount = Id.IntAmount1,
                DateOfBirth = Id.Date1,
                StartOfEmployment = Id.DateRecent1
            };
            var result = await client.PostAsync(_createEmployeeEndpoint, createEmployeeObj.AsJson());

            // Assert
            result.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);

            var content = await result.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeAnonymousType(content, new  {Id = 0});
            obj.Id.Should().NotBe(0);
            
           
        }

        #endregion

        #region Get employee

        private readonly string _getEmployeeEndpoint = "/api/Employees";

        [Fact]
        public async Task Should_ReturnEmployeeWithCorrectId()
        {
            // Arrange
            using var client = await _factory.CreateAuthenticatedHttpClientAsync();

            var createEmployeeObj = new CreateEmployeeModel
            {
                Name = Id.StringValue1,
                Active = true,
                ChildrenCount = Id.IntAmount1,
                DateOfBirth = Id.Date1,
                StartOfEmployment = Id.DateRecent1
            };
            var employeeId = await CreateEmployee(client, createEmployeeObj);

            // Act
            var response = await client.GetAsync($"{_getEmployeeEndpoint}/{employeeId}");

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject<EmployeeModel>(content);

            resultObj.Should().BeEquivalentTo(new EmployeeModel
            {
                Id = employeeId,
                Name = Id.StringValue1,
                Active = true,
                ChildrenCount = Id.IntAmount1,
                DateOfBirth = Id.Date1,
                StartOfEmployment = Id.DateRecent1
            });
        }

        #endregion

        #region Get (list) employee

        [Fact]
        public async Task Should_ReturnAllEmployees()
        {
            // Arrange
            using var client = await _factory.CreateAuthenticatedHttpClientAsync();

            var createEmployeeObj1 = new CreateEmployeeModel
            {
                Name = Id.StringValue1,
                Active = true,
                ChildrenCount = Id.IntAmount1,
                DateOfBirth = Id.Date1,
                StartOfEmployment = Id.DateRecent1
            };
            var createEmployeeObj2 = new CreateEmployeeModel
            {
                Name = Id.StringValue2,
                Active = false,
                ChildrenCount = Id.IntAmount2,
                DateOfBirth = Id.Date2,
                StartOfEmployment = Id.DateRecent2
            };
            var createEmployeeObj3 = new CreateEmployeeModel
            {
                Name = Id.StringValue3,
                Active = true,
                ChildrenCount = Id.IntAmount3,
                DateOfBirth = Id.Date3,
                StartOfEmployment = Id.DateRecent3
            };

            var id1 = await CreateEmployee(client, createEmployeeObj1);
            var id2 = await CreateEmployee(client, createEmployeeObj2);
            var id3 = await CreateEmployee(client, createEmployeeObj3);

            // Act
            var response = await client.GetAsync($"{_getEmployeeEndpoint}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject<List<EmployeeModel>>(content);
            resultObj.Count.Should().BeGreaterOrEqualTo(3);
            resultObj.Should().Contain(model => model.Id == id1);
            resultObj.Should().Contain(model => model.Id == id2);
            resultObj.Should().Contain(model => model.Id == id3);
        }

        #endregion

        #region Get (list) employees older than set age

        private readonly string _getEmployeesOlderThanEndpoint = "api/Employees/listOlderThan";

        [Fact]
        public async Task Should_ReturnEmployeesOlderThanSetAge()
        {
            // Arrange
            var minAge = 18;
            var minAgeDateTime = DateTime.UtcNow.AddYears(-minAge);
            using var client = await _factory.CreateAuthenticatedHttpClientAsync();

            var createEmployeeObj1 = new CreateEmployeeModel
            {
                Name = Id.StringValue1,
                Active = true,
                ChildrenCount = Id.IntAmount1,
                DateOfBirth = minAgeDateTime.AddYears(-1),
                StartOfEmployment = Id.DateRecent1
            };
            var createEmployeeObj2 = new CreateEmployeeModel
            {
                Name = Id.StringValue2,
                Active = false,
                ChildrenCount = Id.IntAmount2,
                DateOfBirth = minAgeDateTime.AddYears(1),
                StartOfEmployment = Id.DateRecent2
            };
            var createEmployeeObj3 = new CreateEmployeeModel
            {
                Name = Id.StringValue3,
                Active = true,
                ChildrenCount = Id.IntAmount3,
                DateOfBirth = minAgeDateTime.AddYears(-2),
                StartOfEmployment = Id.DateRecent3
            };

            var id1 = await CreateEmployee(client, createEmployeeObj1);
            var id2 = await CreateEmployee(client, createEmployeeObj2);
            var id3 = await CreateEmployee(client, createEmployeeObj3);

            // Act
            var response = await client.GetAsync($"{_getEmployeesOlderThanEndpoint}/{minAge}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject<List<EmployeeModel>>(content);
            resultObj.Count.Should().BeGreaterOrEqualTo(2);
            resultObj.Should().Contain(model => model.Id == id1);
            resultObj.Should().NotContain(model => model.Id == id2);
            resultObj.Should().Contain(model => model.Id == id3);
        }

        #endregion

            #region Update employee

        private readonly string _updateEmployeeEndpoint = "/api/Employees";

        [Fact]
        public async Task Should_CorrectlyUpdateEmployee()
        {
            // Arrange
            using var client = await _factory.CreateAuthenticatedHttpClientAsync();

            var createEmployeeObj = new CreateEmployeeModel
            {
                Name = Id.StringValue1,
                Active = true,
                ChildrenCount = Id.IntAmount1,
                DateOfBirth = Id.Date1,
                StartOfEmployment = Id.DateRecent1
            };
            var employeeId = await CreateEmployee(client, createEmployeeObj);

            // Act
            var updateEmployeeObj = new UpdateEmployeeModel
            {
                Id = employeeId,
                Name = Id.StringValue2,
                Active = false,
                ChildrenCount = Id.IntAmount2,
                DateOfBirth = Id.Date2,
                StartOfEmployment = Id.DateRecent2
            };
            var response = await client.PutAsync($"{_updateEmployeeEndpoint}/{employeeId}", updateEmployeeObj.AsJson());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            response = await client.GetAsync($"{_getEmployeeEndpoint}/{employeeId}");
            var content = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject<EmployeeModel>(content);

            resultObj.Should().BeEquivalentTo(updateEmployeeObj);
        }

        #endregion

        #region Employee delete

        private readonly string _deleteEmployeeEndpoint = "/api/Employees";

        [Fact]
        public async Task Should_DeleteAndReturnEmployee()
        {
            // Arrange
            using var client = await _factory.CreateAuthenticatedHttpClientAsync();

            var createEmployeeObj = new CreateEmployeeModel
            {
                Name = Id.StringValue1,
                Active = true,
                ChildrenCount = Id.IntAmount1,
                DateOfBirth = Id.Date1,
                StartOfEmployment = Id.DateRecent1
            };
            var employeeId = await CreateEmployee(client, createEmployeeObj);

            // Act
            var response = await client.DeleteAsync($"{_deleteEmployeeEndpoint}/{employeeId}");

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject<EmployeeModel>(content);

            resultObj.Should().BeEquivalentTo(new EmployeeModel
            {
                Id = employeeId,
                Name = Id.StringValue1,
                Active = true,
                ChildrenCount = Id.IntAmount1,
                DateOfBirth = Id.Date1,
                StartOfEmployment = Id.DateRecent1
            });

            response = await client.GetAsync($"{_getEmployeeEndpoint}/{employeeId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}
