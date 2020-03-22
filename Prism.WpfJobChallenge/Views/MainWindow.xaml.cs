using Prism.Regions;
using System;
using System.Windows;
using Unity;

namespace Prism.WpfJobChallenge.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public MainWindow(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;

            InitializeComponent();

            if (regionManager == null)
            {
                throw new ArgumentNullException(nameof(regionManager));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            regionManager.RegisterViewWithRegion("ContentRegion", typeof(Plot));
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
