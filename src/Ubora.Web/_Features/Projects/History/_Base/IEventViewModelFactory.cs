using System;

namespace Ubora.Web._Features.Projects.History._Base
{
    public interface IEventViewModelFactory
    {
        bool CanCreateFor(Type type);
    }
}
