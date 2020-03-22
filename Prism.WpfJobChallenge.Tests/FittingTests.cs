using Microsoft.VisualStudio.TestTools.UnitTesting;
using OxyPlot;
using Prism.WpfJobChallenge.Tests.Helpers;
using Prism.WpfJobChallenge.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism.WpfJobChallenge.Tests
{
    /// <summary>
    /// This Class is supposed to test the results of the fitting calculations against test data
    /// You can specify:
    /// <see cref="FittingTestData.input"/> => Input Datapoints
    /// <see cref="FittingTestData.expectedOutput"/> => Expected Output Datapoints
    /// <see cref="FittingTestData.expectedA"/> => Expected Output A
    /// <see cref="FittingTestData.expectedB"/> => Expected Output B
    /// <see cref="FittingTestData.accuracy"/> => Actual results and expected Outputs will be rounded on this many digits
    /// 
    /// Add as much Data to the Dictionary as required. 
    /// </summary>
    [TestClass]
    public class FittingTests : BaseTest
    {
        Dictionary<string, FittingTestData> FittingTestData = new Dictionary<string, FittingTestData>
        {
            ///Linear
            { "Linear_001", new FittingTestData(
                4, 
                new List<DataPoint> {
                    new DataPoint(1, 1),
                    new DataPoint(2, 4.5),
                    new DataPoint(3, 8.8),
                    new DataPoint(4, 16.4),
                    new DataPoint(5, 24.4),
                    new DataPoint(6, 36.6),
                    new DataPoint(7, 48.9),
                },

                new List<DataPoint> {
                    new DataPoint(1, -3.8607),
                    new DataPoint(2, 4.1214),
                    new DataPoint(3, 12.1036),
                    new DataPoint(4, 20.0857),
                    new DataPoint(5, 28.0679),
                    new DataPoint(6, 36.05),
                    new DataPoint(7, 44.0321),
                },

                -11.8429 ,
                7.9821
                ) 
            },
                        
            ///Exponential
            { "Exponential_001", new FittingTestData(
                4,
                new List<DataPoint> {
                    new DataPoint(1, 1),
                    new DataPoint(2, 4.5),
                    new DataPoint(3, 8.8),
                    new DataPoint(4, 16.4),
                    new DataPoint(5, 24.4),
                    new DataPoint(6, 36.6),
                    new DataPoint(7, 48.9),
                },

                new List<DataPoint> {
                    new DataPoint(1, 1.5045),
                    new DataPoint(2, 2.7493),
                    new DataPoint(3, 5.0241),
                    new DataPoint(4, 9.1811),
                    new DataPoint(5, 16.7776),
                    new DataPoint(6, 30.6595),
                    new DataPoint(7, 56.0273),
                },

                1.0407 ,
                0.6029
                )
            },
                        
            ///Power
            { "Power_001", new FittingTestData(
                4,
                new List<DataPoint> {
                    new DataPoint(1, 1),
                    new DataPoint(2, 4.5),
                    new DataPoint(3, 8.8),
                    new DataPoint(4, 16.4),
                    new DataPoint(5, 24.4),
                    new DataPoint(6, 36.6),
                    new DataPoint(7, 48.9),
                },

                new List<DataPoint> {
                    new DataPoint(1, 1.0388),
                    new DataPoint(2, 4.0977),
                    new DataPoint(3, 9.1452),
                    new DataPoint(4, 16.1645),
                    new DataPoint(5, 25.1441),
                    new DataPoint(6, 36.0752),
                    new DataPoint(7, 48.9507)
                },

                1.041 ,
                1.9799
                )
            },
        };

        /// <summary>
        /// Use the dictionary key string as parameter to have a meaningful output in the test window instead of arrays of numbers
        /// </summary>
        [DataTestMethod]
        [DataRow("Linear_001")]
        public void ViewModelCalculatesLinearFittingCorrectly(string key)
        {
            TestForExpectedFittedPoints(key, PlotViewModel.LIN);
        }

        /// <summary>
        /// Use the dictionary key string as parameter to have a meaningful output in the test window instead of arrays of numbers
        /// </summary>
        [DataTestMethod]
        [DataRow("Linear_001")]
        public void ViewModelCalculatesLinearParamsCorrectly(string key)
        {
            TestForExpectedFittedParams(key, PlotViewModel.LIN);
        }

        /// <summary>
        /// Use the dictionary key string as parameter to have a meaningful output in the test window instead of arrays of numbers
        /// </summary>
        [DataTestMethod]
        [DataRow("Exponential_001")]
        public void ViewModelCalculatesExponentialFittingCorrectly(string key)
        {
            TestForExpectedFittedPoints(key, PlotViewModel.EXP);
        }

        /// <summary>
        /// Use the dictionary key string as parameter to have a meaningful output in the test window instead of arrays of numbers
        /// </summary>
        [DataTestMethod]
        [DataRow("Exponential_001")]
        public void ViewModelCalculatesExponentialParamsCorrectly(string key)
        {
            TestForExpectedFittedParams(key, PlotViewModel.EXP);
        }

        /// <summary>
        /// Use the dictionary key string as parameter to have a meaningful output in the test window instead of arrays of numbers
        /// </summary>
        [DataTestMethod]
        [DataRow("Power_001")]
        public void ViewModelCalculatesPowerFittingCorrectly(string key)
        {
            TestForExpectedFittedPoints(key, PlotViewModel.POW);
        }

        /// <summary>
        /// Use the dictionary key string as parameter to have a meaningful output in the test window instead of arrays of numbers
        /// </summary>
        [DataTestMethod]
        [DataRow("Power_001")]
        public void ViewModelCalculatesPowerParamsCorrectly(string key)
        {
            TestForExpectedFittedParams(key, PlotViewModel.POW);
        }

        private void TestForExpectedFittedPoints(string key, string fitting)
        {
            if (!FittingTestData.ContainsKey(key))
            {
                throw new ArgumentException("Check your Test Data => " + key + " not found.");
            }

            // Arrange
            var testData = FittingTestData[key];
            var comparer = new DataPointEqualityComparer(testData.accuracy);

            // Act
            viewModel.Points = testData.input;
            viewModel.SelectedFitting = fitting;

            // Assert
            Assert.IsTrue(
                viewModel.FittedPoints.OrderBy(x => x.X)
                .SequenceEqual
                (testData.expectedOutput.OrderBy(x => x.X), comparer));
        }

        private void TestForExpectedFittedParams(string key, string fitting)
        {
            if (!FittingTestData.ContainsKey(key))
            {
                throw new ArgumentException("Check your Test Data => " + key + " not found.");
            }

            // Arrange
            var testData = FittingTestData[key];
            var comparer = new DataPointEqualityComparer(testData.accuracy);

            // Act
            viewModel.Points = testData.input;
            viewModel.SelectedFitting = fitting;

            // Assert
            Assert.IsTrue(
                Math.Round(viewModel.A, testData.accuracy)
                .Equals
                (Math.Round(testData.expectedA, testData.accuracy))
                &&
                Math.Round(viewModel.B, testData.accuracy)
                .Equals
                (Math.Round(testData.expectedB, testData.accuracy)));
        }
    }
}
