using System;
using Ubora.Domain.Queries;

namespace Ubora.Domain.Commands
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}