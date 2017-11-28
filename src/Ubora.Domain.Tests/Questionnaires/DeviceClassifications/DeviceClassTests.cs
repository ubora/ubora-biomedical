using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Types;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.DeviceClassifications
{
    public class DeviceClassTests
    {
        [Fact]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void DeviceClasses_Should_Be_Comparable_By_Risk()
        {
            var expectedOrderFromLowestToHighest = new []
            {
                DeviceClass.None,
                DeviceClass.One,
                DeviceClass.TwoA,
                DeviceClass.TwoB,
                DeviceClass.Three
            };

            var randomOrder = GetAllDeviceClasses().OrderBy(x => Guid.NewGuid());

            // Act
            var result = randomOrder.OrderBy(dc => dc);

            // Assert
            result.Should().Equal(expectedOrderFromLowestToHighest);
        }

        [Fact]
        public void DeviceClasses_Of_Same_Type_Should_Be_Equal()
        {
            foreach (var deviceClass in GetAllDeviceClasses())
            {
                var equalDeviceClass = Activator.CreateInstance(deviceClass.GetType());

                deviceClass.Equals(equalDeviceClass).Should().BeTrue();
            }
        }

        [Fact]
        public void DeviceClasses_Of_Different_Type_Should_Not_Be_Equal()
        {
            var deviceClasses = GetAllDeviceClasses();

            // Act && Assert
            foreach (var deviceClass in deviceClasses)
            {
                var otherDeviceClasses = deviceClasses.Where(dc => dc.GetType() != deviceClass.GetType());

                otherDeviceClasses.ForEach(otherDeviceClass =>
                {
                    deviceClass.Equals(otherDeviceClass).Should().BeFalse();
                });
            }
        }

        private static DeviceClass[] GetAllDeviceClasses()
        {
            var assembly = typeof(DeviceClass).Assembly;

            var deviceClassTypes = AllTypes.From(assembly)
                .ThatDeriveFrom<DeviceClass>()
                .ToArray();

            var deviceClasses = deviceClassTypes
                .Select(type => Activator.CreateInstance(type))
                .Cast<DeviceClass>();

            return deviceClasses.ToArray();
        }
    }
}
