using log4net;
using log4net.Config;
using Prism.Ioc;
using Prism.Unity;
using Prism.WpfJobChallenge.Interfaces;
using Prism.WpfJobChallenge.Services;
using Prism.WpfJobChallenge.Views;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Unity;
using Unity.log4net;

namespace Prism.WpfJobChallenge
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureLog4Net();

            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        private void ConfigureLog4Net()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDatapointsService, SimpleWindowsXMLFileDatapointsService>();
            containerRegistry.Register<INotificationService, SimpleWindowsMessageBoxNotificationService>();

            IUnityContainer unityContainer = containerRegistry.GetContainer();
            unityContainer.AddExtension(new Log4NetExtension());
        }
    }
}