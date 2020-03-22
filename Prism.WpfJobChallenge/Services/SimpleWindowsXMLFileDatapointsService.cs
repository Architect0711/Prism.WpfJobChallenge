using Microsoft.Win32;
using OxyPlot;
using Prism.Mvvm;
using Prism.WpfJobChallenge.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Prism.WpfJobChallenge.Services
{
    public class SimpleWindowsXMLFileDatapointsService : BindableBase, IDatapointsService
    {
        INotificationService _notificationService;

        public SimpleWindowsXMLFileDatapointsService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private string _DatapointsSource;
        public string DatapointsSource
        {
            get { return _DatapointsSource; }
            set
            {
                SetProperty(ref _DatapointsSource, value, nameof(DatapointsSource));
            }
        }
        public List<DataPoint> GetDatapoints()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "XML files (*.xml) | *.xml";
                openFileDialog.CheckPathExists = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.DefaultExt = ".xml";
                openFileDialog.InitialDirectory = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "SampleData");

                if (openFileDialog.ShowDialog() == true)
                {
                    return GetDatapoints(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                NotifyUser(ex.ToString());
            }

            return null;
        }

        private void NotifyUser(string message)
        {
            _notificationService.Notify("Error", message, Enums.NotificationType.Error);
        }

        public List<DataPoint> GetDatapoints(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                NotifyUserFileNotFound("Path was empty");
                throw new ArgumentException(source);
            }

            if (File.Exists(source))
            {
                XDocument doc = null;

                try
                {
                    doc = XDocument.Load(source);
                }
                catch
                {
                    throw new ArgumentException(source);
                }

                if (doc != null)
                {
                    List<XElement> points = new List<XElement>();

                    try
                    {
                        points = doc.Element("Points")
                            .Elements()
                            .Where(x => x.Name == "Point")
                            .ToList();
                    }
                    catch
                    {
                        NotifyUserOfInvalidXML();
                        throw new ArgumentException(source);
                    }

                    List<DataPoint> newDataset = new List<DataPoint>();

                    foreach (var point in points)
                    {
                        string x = "";
                        string y = "";  
                        if (point.Attribute("X") != null && point.Attribute("Y") != null)
                        {
                            x = point.Attribute("X").Value;
                            y = point.Attribute("Y").Value;
                        }
                        else
                        {
                            NotifyUserOfInvalidXML();
                            throw new ArgumentException(source);
                        }

                        if (double.TryParse(x, out double _x) && double.TryParse(y, out double _y))
                        {
                            newDataset.Add(new DataPoint(_x, _y));
                        }
                        else
                        {
                            NotifyUserOfInvalidXML();
                            throw new ArgumentException(source);
                        }
                    }

                    DatapointsSource = Path.GetFileName(source);
                    return newDataset;
                }
            }

            NotifyUserFileNotFound(source);
            throw new FileNotFoundException(source);
        }

        private void NotifyUserFileNotFound(string path)
        {
            string message =
                   "File could not be found:" +
                   Environment.NewLine +
                   Environment.NewLine +
                   path +
                   Environment.NewLine +
                   Environment.NewLine +
                   "Please provide a valid Path to an XML File";

            NotifyUser(message);
        }

        private void NotifyUserOfInvalidXML()
        {
            string message =
                   "Dataset could not be processed" +
                   Environment.NewLine +
                   Environment.NewLine +
                   "Please provide an XML File in the following Format:" +
                   Environment.NewLine +
                   Environment.NewLine +
                   "<Points>" +
                   Environment.NewLine +
                   "   <Point X=\"10\" Y=\"11\"/>" +
                   Environment.NewLine +
                   "   <Point X=\"20\" Y=\"20\"/>" +
                   Environment.NewLine +
                   "   <Point X=\"30\" Y=\"26\"/>" +
                   Environment.NewLine +
                   "   ..." +
                   Environment.NewLine +
                   "</Points>" +
                   Environment.NewLine;

            NotifyUser(message);
        }
    }
}