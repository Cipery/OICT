using AutoMapper;
using NUnit.Framework;
using OICT.Application;

namespace OICT.Api.Application.Tests
{
    public class MappingTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestAutomapperMappings()
        {
            var mapperConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));
            mapperConfig.AssertConfigurationIsValid();
        }
    }
}