using Prism.WpfJobChallenge.Enums;
using Prism.WpfJobChallenge.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Prism.WpfJobChallenge.Tests.Mocks
{
    public class MockNotificationService : INotificationService
    {
        public void Notify(string message, NotificationType notificationType)
        {
            Notify(string.Empty, message, notificationType);
        }

        public void Notify(string caption, string message, NotificationType notificationType)
        {
            Debug.WriteLine("[" + Enum.GetName(typeof(NotificationType), notificationType) + "]"
                           + "[" + caption + "] "
                           + message);
        }
    }
}
