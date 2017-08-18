using System;

namespace Ubora.Web._Features.Notifications._Base
{
    public interface INotificationViewModelFactory
    {
        bool CanCreateFor(Type type);
    }
}