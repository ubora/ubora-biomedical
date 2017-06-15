using System.Linq;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public interface IDeviceClassificationProvider
    {
        IDeviceClassification Provide();
    }

    public class DeviceClassificationProvider : IDeviceClassificationProvider
    {
        private ICommandQueryProcessor _processor;

        public DeviceClassificationProvider(ICommandQueryProcessor processor)
        {
            _processor = processor;
        }

        public IDeviceClassification Provide()
        {
            return _processor.Find<DeviceClassification>().Single();
        }
    }
}
