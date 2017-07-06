using AutoMapper;
using Ubora.Web.Infrastructure;
using Xunit;

namespace Ubora.Web.Tests.Infrastructure
{
    public class AutoMapperTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Configuration_Is_Valid(bool useSpecifiedPickupDirectory)
        {
            var domainAutofacModule = new WebAutofacModule(useSpecifiedPickupDirectory);
            Mapper.Initialize(cfg => domainAutofacModule.AddAutoMapperProfiles(cfg));

            // Act
            Mapper.AssertConfigurationIsValid();
        }
    }
}
