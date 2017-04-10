namespace Ubora.Domain.Commands
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}