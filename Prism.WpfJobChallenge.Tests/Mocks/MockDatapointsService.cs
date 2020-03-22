using OxyPlot;
using Prism.WpfJobChallenge.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.WpfJobChallenge.Tests.Mocks
{
    public class MockDatapointsService : IDatapointsService
    {
        public string DatapointsSource => "Mock";

        public List<DataPoint> GetDatapoints()
        {
            return new List<DataPoint>
            {
                new DataPoint(10, 10),
                new DataPoint(20, 20),
                new DataPoint(30, 30),
                new DataPoint(40, 40),
                new DataPoint(50, 50),
                new DataPoint(60, 60),
                new DataPoint(70, 70),
                new DataPoint(80, 80),
                new DataPoint(90, 90),
                new DataPoint(100,100)
            };
        }

        public List<DataPoint> GetDatapoints(string source)
        {
            return GetDatapoints();
        }
    }
}
