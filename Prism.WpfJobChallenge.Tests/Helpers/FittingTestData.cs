using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.WpfJobChallenge.Tests.Helpers
{
    public class FittingTestData
    {
        public readonly int accuracy;
        public readonly List<DataPoint> input;
        public readonly List<DataPoint> expectedOutput;
        public readonly double expectedA;
        public readonly double expectedB;

        public FittingTestData(int accuracy, List<DataPoint> input, List<DataPoint> expectedOutput, double expectedA, double expectedB)
        {
            this.accuracy = accuracy;
            this.input = input;
            this.expectedOutput = expectedOutput;
            this.expectedA = expectedA;
            this.expectedB = expectedB;
        }
    }
}
