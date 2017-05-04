using AutoMapper;
using Ubora.Web.Infrastructure;
using Xunit;

namespace Ubora.Web.Tests.Infrastructure
{
    public class AutoMapperTests
    {
        [Fact]
        public void Configuration_Is_Valid()
        {
            var domainAutofacModule = new WebAutofacModule();
            Mapper.Initialize(cfg => domainAutofacModule.AddAutoMapperProfiles(cfg));

            // Act
            Mapper.AssertConfigurationIsValid();
        }
    }
}
