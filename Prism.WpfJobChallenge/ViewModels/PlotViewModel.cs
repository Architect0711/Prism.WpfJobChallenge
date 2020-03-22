using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using OxyPlot;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using System.Windows;
using System.Xml.Linq;
using log4net;
using Prism.WpfJobChallenge.Interfaces;

namespace Prism.WpfJobChallenge.ViewModels
{
    public class PlotViewModel : BindableBase
    {
        private readonly IDatapointsService _datapointsService;
        private readonly INotificationService _notificationService;

        private List<DataPoint> _Points;
        public List<DataPoint> Points
        {
            get { return _Points; }
            set
            {
                SetProperty(ref _Points, value, nameof(Points));
                RaisePropertyChanged(nameof(CanSelectFitting));
            }
        }

        private DataPoint _SelectedPoint;
        public DataPoint SelectedPoint
        {
            get { return _SelectedPoint; }
            set
            {
                SetProperty(ref _SelectedPoint, value, nameof(SelectedPoint));
                try
                {
                    if (FittedPoints != null)
                    {
                        var fittedPoint = FittedPoints
                            .FirstOrDefault(pt => pt.X == SelectedPoint.X);
                        if (!fittedPoint.Equals(default(DataPoint)))
                        {
                            SelectedFittedPoint = fittedPoint;
                            Error = SelectedPoint.Y - SelectedFittedPoint.Y;
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private List<DataPoint> _FittedPoints;
        public List<DataPoint> FittedPoints
        {
            get { return _FittedPoints; }
            set { SetProperty(ref _FittedPoints, value, nameof(FittedPoints)); }
        }

        private DataPoint _SelectedFittedPoint;
        public DataPoint SelectedFittedPoint
        {
            get { return _SelectedFittedPoint; }
            set
            {
                SetProperty(ref _SelectedFittedPoint, value, nameof(SelectedFittedPoint));
            }
        }

        public bool CanSelectFitting
        {
            get { return (Points != null && Points.Count > 0); }
        }

        private double _A;
        public double A
        {
            get { return _A; }
            set { SetProperty(ref _A, value, nameof(A)); }
        }

        private double _B;
        public double B
        {
            get { return _B; }
            set { SetProperty(ref _B, value, nameof(B)); }
        }

        private double _Error;
        public double Error
        {
            get { return _Error; }
            set { SetProperty(ref _Error, value, nameof(Error)); }
        }

        public static int ShowDecimals;

        private ObservableCollection<string> _AvailableFittings;
        public ObservableCollection<string> AvailableFittings
        {
            get { return _AvailableFittings; }
            set { SetProperty(ref _AvailableFittings, value, nameof(AvailableFittings)); }
        }

        private string _SelectedFitting;
        public string SelectedFitting
        {
            get { return _SelectedFitting; }
            set 
            {
                SetProperty(ref _SelectedFitting, value, nameof(SelectedFitting));
                CalculateFitting();
            }
        }

        private string _CalculatedFitting;
        public string CalculatedFitting
        {
            get { return _CalculatedFitting; }
            set
            {
                SetProperty(ref _CalculatedFitting, value, nameof(CalculatedFitting));
            }
        }

        public const string LIN = "Linear: y = (a * x) + b";
        public const string EXP = "Exponential: y = a * exp(b * x)";
        public const string POW = "Power function: y = a * (x ^ b)";

        private string _SelectedDataset;
        public string SelectedDataset
        {
            get { return _SelectedDataset; }
            set
            {
                SetProperty(ref _SelectedDataset, value, nameof(SelectedDataset));
                CalculateFitting();
            }
        }
        
        public DelegateCommand<object> OpenDataSetCommand { get; private set; }
        void OpenDataSet(object parameter)
        {
            try
            {
                var points = _datapointsService.GetDatapoints();

                if (points != null)
                {
                    Points = points.ToList();
                    SelectedDataset = _datapointsService.DatapointsSource;
                }
            }
            catch (Exception ex)
            {
                _notificationService.Notify("Error", ex.ToString(), Enums.NotificationType.Error);
            }
        }

        public PlotController customController { get; private set; }

        public PlotViewModel(IDatapointsService datapointsService, INotificationService notificationService)
        { 
            _datapointsService = datapointsService;
            _notificationService = notificationService;

            ShowDecimals = 4;

            AvailableFittings = new ObservableCollection<string>();
            AvailableFittings.Add(LIN);
            AvailableFittings.Add(EXP);
            AvailableFittings.Add(POW);

            OpenDataSetCommand = new DelegateCommand<object>(OpenDataSet);
        }

        private void CalculateFitting()
        {
            if (!string.IsNullOrWhiteSpace(SelectedFitting))
            {
                switch (SelectedFitting)
                {
                    case LIN:
                        ApplyLinearFitting();
                        break;
                    case EXP:
                        ApplyExponentialFitting();
                        break;
                    case POW:
                        ApplyPowerFunctionFitting();
                        break;
                    default:
                        throw new NotImplementedException(SelectedFitting);
                }

                CalculatedFitting = SelectedFitting
                        .Replace("a ", Math.Round(A, ShowDecimals).ToString() + " ")
                        .Replace("b", Math.Round(B, ShowDecimals).ToString());
            }
        }

        /// https://numerics.mathdotnet.com/Regression.html#Evaluating-the-model-at-specific-data-points
        /// Linear: y = (a * x) + b
        private void ApplyLinearFitting()
        {
            double[] xdata = Points.Select(p => p.X).ToArray();
            double[] ydata = Points.Select(p => p.Y).ToArray();

            Tuple<double, double> p = Fit.Line(xdata, ydata);
            double a = p.Item1; // == 10; intercept
            double b = p.Item2; // == 0.5; slope

            A = a;
            B = b;

            //Func<double, double> f = Fit.LineFunc(xdata, ydata);

            Func<double, double> f = Fit.LinearCombinationFunc(
                xdata,
                ydata,
                x => a*x,
                x => b);

            List<DataPoint> fittedPoints = new List<DataPoint>();

            foreach (var item in Points)
            {
                fittedPoints.Add(new DataPoint(item.X, f(item.X)));
            }

            FittedPoints = fittedPoints;
        }

        /// https://numerics.mathdotnet.com/Regression.html#Evaluating-the-model-at-specific-data-points
        /// https://numerics.mathdotnet.com/Regression.html#Linearizing-non-linear-models-by-transformation
        /// https://discuss.mathdotnet.com/t/exponential-fit/131/2
        /// Exponential: y = a * exp(b * x)
        private void ApplyExponentialFitting()
        {
            double[] x = Points.Select(p => p.X).ToArray();
            double[] y = Points.Select(p => p.Y).ToArray();
            double[] p = Exponential(x, y); // a=1.017, r=0.687

            A = p[0];
            B = p[1];

            Func<double, double> f = Fit.LinearCombinationFunc(
                Points.Select(p => p.X).ToArray(),
                Points.Select(p => p.Y).ToArray(),
                x => p[0] * Math.Exp(p[1]*x));


            List<DataPoint> fittedPoints = new List<DataPoint>();

            foreach (var item in Points)
            {
                fittedPoints.Add(new DataPoint(item.X, f(item.X)));
            }

            FittedPoints = fittedPoints;
        }

        private double[] Exponential(double[] x, double[] y, DirectRegressionMethod method = DirectRegressionMethod.QR)
        {
            double[] y_hat = Generate.Map(y, Math.Log);
            double[] p_hat = Fit.LinearCombination(x, y_hat, method, t => 1.0, t => t);
            return new[] { Math.Exp(p_hat[0]), p_hat[1] };
        }

        /// https://numerics.mathdotnet.com/Regression.html#Linearizing-non-linear-models-by-transformation
        /// https://github.com/mathnet/mathnet-numerics/blob/master/src/Numerics/Fit.cs
        /// https://discuss.mathdotnet.com/t/exponential-fit/131/2
        /// https://discuss.mathdotnet.com/t/curve-fitting-power/605
        /// Power function: y = a * (x ^ b)
        private void ApplyPowerFunctionFitting()
        {
            double[] x = Points.Select(p => p.X).ToArray();
            double[] y = Points.Select(p => p.Y).ToArray();
            Tuple<double, double> p = Fit.Power(x, y); // a=1.017, r=0.687

            A = p.Item1;
            B = p.Item2;

            Func<double, double> f = Fit.LinearCombinationFunc(
                Points.Select(p => p.X).ToArray(),
                Points.Select(p => p.Y).ToArray(),
                x => p.Item1 * Math.Pow(x, p.Item2));
            
            List<DataPoint> fittedPoints = new List<DataPoint>();

            foreach (var item in Points)
            {
                fittedPoints.Add(new DataPoint(item.X, f(item.X)));
            }

            FittedPoints = fittedPoints;
        }
    }
}
