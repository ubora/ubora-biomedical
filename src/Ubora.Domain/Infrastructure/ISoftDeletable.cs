namespace Ubora.Domain.Infrastructure
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; }
    }
}
