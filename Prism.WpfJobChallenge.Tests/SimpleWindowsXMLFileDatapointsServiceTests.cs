using Microsoft.VisualStudio.TestTools.UnitTesting;
using OxyPlot;
using Prism.WpfJobChallenge.Interfaces;
using Prism.WpfJobChallenge.Services;
using Prism.WpfJobChallenge.Tests.Helpers;
using Prism.WpfJobChallenge.Tests.Mocks;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Prism.WpfJobChallenge.Tests
{
    [TestClass]
    public class SimpleWindowsXMLFileDatapointsServiceTests
    {
        INotificationService notificationService;

        Dictionary<string, XMLTestData> validXMLFiles = new Dictionary<string, XMLTestData>
        {
            {
                "Dataset_001", 
                new XMLTestData(
                    new List<DataPoint>
                    {
                        new DataPoint(10,11),
                        new DataPoint(20,20),
                        new DataPoint(30,26),
                        new DataPoint(40,36),
                        new DataPoint(50,54),
                        new DataPoint(60,59),
                        new DataPoint(70,72)
                    },
                    "Dataset_001.xml")
            },
            {
                "Dataset_002",
                new XMLTestData(
                    new List<DataPoint>
                    {
                        new DataPoint(1,1),
                        new DataPoint(2,4.5),
                        new DataPoint(3,8.8),
                        new DataPoint(4,16.4),
                        new DataPoint(5,24.4),
                        new DataPoint(6,36.6),
                        new DataPoint(7,48.9)
                    },
                    "Dataset_002.xml")
            },
            {
                "Dataset_003",
                new XMLTestData(
                    new List<DataPoint>
                    {
                        new DataPoint(1,1),
                        new DataPoint(2,3),
                        new DataPoint(3,4),
                        new DataPoint(4,4.5),
                        new DataPoint(5,4.7),
                        new DataPoint(6,4.8),
                        new DataPoint(7,4.9)
                    },
                    "Dataset_003.xml")
            }
        };

        Dictionary<string, XMLTestData> invalidXMLFiles = new Dictionary<string, XMLTestData>
        {
            {
                "Dataset_004",
                new XMLTestData(
                    new List<DataPoint>(),
                    "Dataset_004.xml")
            },
            {
                "Dataset_005",
                new XMLTestData(
                    new List<DataPoint>(),
                    "Dataset_005.xml")
            },
            {
                "Dataset_006",
                new XMLTestData(
                    new List<DataPoint>(),
                    "Dataset_006.xml")
            }
        };

        public SimpleWindowsXMLFileDatapointsServiceTests()
        {
            notificationService = new MockNotificationService();
        }

        [DataTestMethod]
        [DataRow("Dataset_001")]
        [DataRow("Dataset_002")]
        [DataRow("Dataset_003")]
        public void ServiceReadsValidXMLFilesCorrectly(string key)
        {
            if (!validXMLFiles.ContainsKey(key))
            {
                throw new ArgumentException("Check your Test Data => " + key + " not found.");
            }

            // Arrange
            IDatapointsService xmlFileService = new SimpleWindowsXMLFileDatapointsService(notificationService);
            var testData = validXMLFiles[key];

            // Act
            var dataPoints = xmlFileService
                .GetDatapoints(testData.filePath);

            // Assert
            Assert.IsTrue(
                dataPoints.OrderBy(x => x.X)
                .SequenceEqual
                (testData.expectedOutput.OrderBy(x => x.X)));
        }

        [DataTestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow("Dataset_004")]
        [DataRow("Dataset_005")]
        [DataRow("Dataset_006")]
        public void ServiceDetectsInvalidXMLFilesCorrectly(string key)
        {
            // Arrange
            IDatapointsService xmlFileService = new SimpleWindowsXMLFileDatapointsService(notificationService);
            var testData = invalidXMLFiles[key];

            // Act
            var dataPoints = xmlFileService
                .GetDatapoints(testData.filePath);
        }

        [DataTestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        [DataRow("Dataset_007")]
        [DataRow("Dataset_008")]
        [DataRow("Dataset_009")]
        public void ServiceDetectsMissingXMLFilesCorrectly(string key)
        {
            // Arrange
            IDatapointsService xmlFileService = new SimpleWindowsXMLFileDatapointsService(notificationService);
            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "TestFiles",
                key);

            // Act
            var dataPoints = xmlFileService
                .GetDatapoints(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ServiceDetectsEmptyPathCorrectly()
        {
            // Arrange
            IDatapointsService xmlFileService = new SimpleWindowsXMLFileDatapointsService(notificationService);

            // Act
            var dataPoints = xmlFileService
                .GetDatapoints(null);
        }
    }
}
