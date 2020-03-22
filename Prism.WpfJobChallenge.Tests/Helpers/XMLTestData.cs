using OxyPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Prism.WpfJobChallenge.Tests.Helpers
{
    public class XMLTestData
    {
        public readonly List<DataPoint> expectedOutput;
        public readonly string filePath;

        public XMLTestData(List<DataPoint> expectedOutput, string filePath)
        {
            this.expectedOutput = expectedOutput;
            this.filePath = 
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, 
                    "TestFiles",
                    filePath);
        }
    }
}
