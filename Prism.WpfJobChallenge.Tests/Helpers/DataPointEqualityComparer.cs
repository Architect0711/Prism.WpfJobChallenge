using OxyPlot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Prism.WpfJobChallenge.Tests.Helpers
{
    public class DataPointEqualityComparer : IEqualityComparer<DataPoint>
    {
        int _accuracy;

        public DataPointEqualityComparer(int accuracy)
        {
            _accuracy = accuracy;
        }

        public bool Equals(DataPoint x, DataPoint y)
        {
            return
                x.X.Equals(y.X) &&
                Math.Round(x.Y, _accuracy)
                .Equals(Math.Round(y.Y, _accuracy));
        }

        public int GetHashCode(DataPoint obj)
        {
            return HashCode.Combine(obj.X, Math.Round(obj.Y, _accuracy));
        }
    }
}
