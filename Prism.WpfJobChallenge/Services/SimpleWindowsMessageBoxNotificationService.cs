using Prism.WpfJobChallenge.Enums;
using Prism.WpfJobChallenge.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Prism.WpfJobChallenge.Services
{
    public class SimpleWindowsMessageBoxNotificationService : INotificationService
    {
        public void Notify(string message, NotificationType notificationType)
        {
            Notify(string.Empty, message, notificationType);
        }

        public void Notify(string caption, string message, NotificationType notificationType)
        {
            MessageBoxImage img;

            switch (notificationType)
            {
                case NotificationType.Unknown:
                    img = MessageBoxImage.None;
                    break;
                case NotificationType.Info:
                    img = MessageBoxImage.Asterisk;
                    break;
                case NotificationType.Warn:
                    img = MessageBoxImage.Warning;
                    break;
                case NotificationType.Error:
                    img = MessageBoxImage.Error;
                    break;
                default:
                    throw new ArgumentException();
            }

            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}