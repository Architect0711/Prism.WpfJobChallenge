using Prism.WpfJobChallenge.Interfaces;
using Prism.WpfJobChallenge.Tests.Mocks;
using Prism.WpfJobChallenge.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.WpfJobChallenge.Tests
{
    public class BaseTest
    {
        protected PlotViewModel viewModel;
        protected INotificationService notificationService;
        protected IDatapointsService datapointsService;

        public BaseTest()
        {
            notificationService = new MockNotificationService();
            datapointsService = new MockDatapointsService();
            viewModel = new PlotViewModel(datapointsService, notificationService);
        }
    }
}