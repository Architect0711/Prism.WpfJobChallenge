using Prism.WpfJobChallenge.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.WpfJobChallenge.Interfaces
{
    /// <summary>
    /// Provide the User with some feedback
    /// </summary>
    public interface INotificationService
    {
        void Notify(string message, NotificationType notificationType);

        void Notify(string caption, string message, NotificationType notificationType);
    }
}
