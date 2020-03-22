using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.WpfJobChallenge.Interfaces
{
    /// <summary>
    /// Provides an <see cref="List{DataPoint}"/>
    /// </summary>
    public interface IDatapointsService
    {
        string DatapointsSource { get; }

        List<DataPoint> GetDatapoints();

        List<DataPoint> GetDatapoints(string source);
    }
}