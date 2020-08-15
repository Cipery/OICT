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

namespace OICT.Infrastructure.Tests.Common
{
    /// <summary>
    /// Repository is an abstract class, so it cannot be directly tested. That is why we use EmployeeRepository class, which
    /// extends Repository, to test methods defined in the Repository class
    /// </summary>
    class RepositoryTest : SqliteInMemoryTestBase
    {
        [Test]
        public async Task ListAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var empl1 = new EmployeeEntity
            {
                Id = Id.Id1,
                Name = Id.StringValue1,
                ChildrenCount = 0,
                Active = true,
                DateOfBirth = Id.Date1,
                StartOfEmployment = Id.DateRecent1
            };
            var empl2 = new EmployeeEntity
            {
                Id = Id.Id2,
                Name = Id.StringValue2,
                ChildrenCount = Id.IntAmount1,
                Active = true,
                DateOfBirth = Id.Date2,
                StartOfEmployment = Id.DateRecent2
            };
            var empl3 = new EmployeeEntity
            {
                Id = Id.Id3,
                Name = Id.StringValue3,
                ChildrenCount = Id.IntAmount2,
                Active = false,
                DateOfBirth = Id.Date3,
                StartOfEmployment = Id.DateRecent3
            };

            var employeeList = new List<EmployeeEntity>()
            {
                empl1,
                empl2,
                empl3
            };

            UnitOfWork.Context.Employee.AddRange(employeeList);

            await SaveAndRecreateUnitOfWork();

            // Act

            var repository = new EmployeeRepository(UnitOfWork, Mock.Of<IClock>());
            var result = await repository.ListAsync();


            // Assert
            result.Should().BeEquivalentTo(employeeList);
        }

        // TODO: Test all methods from Repository class
    }
}
