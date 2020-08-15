using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using OICT.Domain.Model;
using OICT.Infrastructure.Common;
using OICT.Infrastructure.Repositories;
using OICT.Tests.Common;

namespace OICT.Infrastructure.Tests.Repositories
{
    class EmployeeRepositoryTest : SqliteInMemoryTestBase
    {
        [Test]
        public async Task ListEmployeesOlderThanAsync_WhenYoungerThanRequestedExists_ShouldReturnOnlyOlder()
        {
            // Arrange
            var requestedMinimumAge = Id.IntAge1;
            var now = DateTime.UtcNow;

            var employeeOkAge1 = new EmployeeEntity
            {
                Id = Id.Id1,
                Name = Id.StringValue1,
                ChildrenCount = 0,
                Active = true,
                DateOfBirth = now.AddYears(-requestedMinimumAge - 1),
                StartOfEmployment = Id.DateRecent1
            };
            var employeeOkAge2 = new EmployeeEntity
            {
                Id = Id.Id2,
                Name = Id.StringValue2,
                ChildrenCount = Id.IntAmount1,
                Active = true,
                DateOfBirth = now.AddYears(-requestedMinimumAge - 5),
                StartOfEmployment = Id.DateRecent2
            };
            var employeeWrongAge1 = new EmployeeEntity
            {
                Id = Id.Id3,
                Name = Id.StringValue3,
                ChildrenCount = Id.IntAmount2,
                Active = false,
                DateOfBirth = now.AddYears(-requestedMinimumAge),
                StartOfEmployment = Id.DateRecent3
            };
            var employeeWrongAge2 = new EmployeeEntity
            {
                Id = Id.Id4,
                Name = Id.StringValue4,
                ChildrenCount = Id.IntAmount3,
                Active = false,
                DateOfBirth = now.AddYears(-requestedMinimumAge + 1),
                StartOfEmployment = Id.DateRecent4
            };

            var employeeList = new List<EmployeeEntity>()
            {
                employeeOkAge1,
                employeeOkAge2,
                employeeWrongAge1,
                employeeWrongAge2
            };

            UnitOfWork.Context.Employee.AddRange(employeeList);

            var clockMock = new Mock<IClock>();
            clockMock.Setup(clock => clock.UtcNow).Returns(now);

            await SaveAndRecreateUnitOfWork();

            // Act
            var repository = new EmployeeRepository(UnitOfWork, clockMock.Object);
            var filteredEmployees = await repository.ListEmployeesOlderThanAsync(requestedMinimumAge);

            // Assert
            filteredEmployees.Should().BeEquivalentTo(employeeOkAge1, employeeOkAge2);
        }
    }
}
