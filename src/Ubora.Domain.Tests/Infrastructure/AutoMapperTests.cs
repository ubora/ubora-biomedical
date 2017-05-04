using AutoMapper;
using Ubora.Domain.Infrastructure;
using Xunit;

namespace Ubora.Domain.Tests.Infrastructure
{
    public class AutoMapperTests
    {
        [Fact]
        public void Configuration_Is_Valid()
        {
            var domainAutofacModule = new DomainAutofacModule("dummyConnectionString");
            Mapper.Initialize(cfg => domainAutofacModule.AddAutoMapperProfiles(cfg));

            // Act
            Mapper.AssertConfigurationIsValid();
        }
    }
}
