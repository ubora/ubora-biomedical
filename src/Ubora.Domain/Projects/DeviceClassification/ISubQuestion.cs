namespace Ubora.Domain.Projects.DeviceClassification
{
    public interface ISubQuestion
    {
        BaseQuestion ParentQuestion { get; }
    }
}
