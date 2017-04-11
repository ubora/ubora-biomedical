namespace Ubora.Domain.Infrastructure.Commands
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}