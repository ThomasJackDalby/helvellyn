using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using Scallop;
using Sloth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Helvellyn.Operations
{
    public class Graph : IOperation
    {
        private static Logger logger = Logger.GetLogger(typeof(Graph));
        public string FullCommand { get { return "graph"; } }
        public string ShortCommand { get { return "g"; } }
        public string Description { get { return "graph transaction information"; } }
        public IArgument[] Arguments { get { return arguments; } }

        private IArgument[] arguments;

        private string tag { get; set; }
        private string scope { get; set; }
        private bool isMonth { get; set; }


        public Graph()
        {
            arguments = new IArgument[]
            {
                new Argument<string>("-tag", v => tag = (string)v, defaultvalue:"all"),
                new Argument<string>("-scope", v => scope = (string)v),
                new Argument<bool>("-month", v => isMonth = (bool)v),
            };
        }

        public void Process()
        {
            IList<Transaction> transactions;
            if (isMonth)
            {
                string[] date = scope.Split('-');
                DateTime month = DateTime.ParseExact(date[0], "MMM", CultureInfo.CurrentCulture);
                int year = Int32.Parse(date[1]);
                DateTime start = new DateTime(year, month.Month, 1);
                DateTime end = start.AddMonths(1).Subtract(TimeSpan.FromDays(1));
                transactions = Program.DataStore.GetTransactionsBetween(start, end);
            }
            else transactions = getAll();

            TagManager.Process(transactions);

            ((List<Transaction>)transactions).Sort((a, b) => a.Date.CompareTo(b.Date));

            if (tag == "all")
            {
                logger.Debug("tag is all");
                Dictionary<string, IList<double[]>> seriess = new Dictionary<string, IList<double[]>>();
                List<double[]> points = new List<double[]>();

                foreach (Transaction t in transactions) points.Add(new double[] { DateTimeAxis.ToDouble(t.Date), t.Balance });
                seriess["all"] = points;
                plot("test", seriess);
            }
            else
            {
                IList<Transaction> filtered = (from transaction in transactions where transaction.Tag == tag select transaction).ToList();
                Transaction.Display(filtered);
            }
        }


        static IList<Transaction> getAll()
        {
            IList<Transaction> transactions = Program.DataStore.GetAllTransactions();
            return transactions;
        }
        static IList<Transaction> getMonth(string monthString, string yearString)
        {
            DateTime month = DateTime.ParseExact(monthString, "MMM", CultureInfo.CurrentCulture);
            int year = Int32.Parse(yearString);

            DateTime start = new DateTime(year, month.Month, 1);
            DateTime end = start.AddMonths(1).Subtract(TimeSpan.FromDays(1));
            logger.Debug("Getting transactions between {0} and {1}", start, end);
            IList<Transaction> transactions = Program.DataStore.GetTransactionsBetween(start, end);
            return transactions;
        }
        static IList<Transaction> getWeek(string weekString, string yearString)
        {
            int week = Int32.Parse(weekString);
            int year = Int32.Parse(yearString);

            DateTime start = new DateTime(year, 1, 1).AddDays(7 * week);
            DateTime end = start.AddMonths(1);
            IList<Transaction> transactions = Program.DataStore.GetTransactionsBetween(start, end);
            return transactions;
        }

        static void plotBars(string title, Dictionary<string, IList<double[]>> seriess)
        {
            logger.Debug("plotting");

            OxyPlot.WindowsForms.PlotView plot = new OxyPlot.WindowsForms.PlotView();
            plot.Dock = DockStyle.Fill;
            PlotModel model = new PlotModel();
            model.Title = title;

            DateTimeAxis xAxis = new DateTimeAxis();
            xAxis.CalendarWeekRule = CalendarWeekRule.FirstFullWeek;
            xAxis.Position = AxisPosition.Bottom;
            xAxis.StringFormat = "dd-MMM-yyyy";
            xAxis.IntervalType = DateTimeIntervalType.Months;
            xAxis.MinorGridlineStyle = LineStyle.Dash;
            xAxis.MajorGridlineStyle = LineStyle.Dash;
            model.Axes.Add(xAxis);

            LinearAxis yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.Unit = "£";
            yAxis.MinorGridlineThickness = 0.5;
            yAxis.MajorGridlineThickness = 2;
            yAxis.MinorStep = 250;
            yAxis.MajorStep = 1000;
            yAxis.MinorGridlineStyle = LineStyle.Dash;
            yAxis.MajorGridlineStyle = LineStyle.Dash;
            yAxis.StringFormat = "£";
            model.Axes.Add(yAxis);

            int catagory = 0;
            foreach (string seriesName in seriess.Keys)
            {
                ColumnSeries series = new ColumnSeries();
                series.Title = title;
                foreach (double t in seriess[seriesName])
                {
                    series.Items.Add(new ColumnItem(t ,catagory));
                }
                model.Series.Add(series);
            }

            plot.Model = model;
            plot.Invalidate();

            Form form = new Form();
            form.Controls.Add(plot);

            form.ShowDialog();
        }

        static void plot(string title, Dictionary<string, IList<double[]>> seriess)
        {
            logger.Debug("plotting");

            OxyPlot.WindowsForms.PlotView plot = new OxyPlot.WindowsForms.PlotView();
            plot.Dock = DockStyle.Fill;
            PlotModel model = new PlotModel();
            model.Title = title;

            DateTimeAxis xAxis = new DateTimeAxis();
            xAxis.CalendarWeekRule = CalendarWeekRule.FirstFullWeek;
            xAxis.Position = AxisPosition.Bottom;
            xAxis.StringFormat = "dd-MMM-yyyy";
            xAxis.IntervalType = DateTimeIntervalType.Months;
            xAxis.MinorGridlineStyle = LineStyle.Dash;
            xAxis.MajorGridlineStyle = LineStyle.Dash;
            model.Axes.Add(xAxis);

            LinearAxis yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.Unit = "£";
            yAxis.MinorGridlineThickness = 0.5;
            yAxis.MajorGridlineThickness = 2;
            yAxis.MinorStep = 250;
            yAxis.MajorStep = 1000;
            yAxis.MinorGridlineStyle = LineStyle.Dash;
            yAxis.MajorGridlineStyle = LineStyle.Dash;
            yAxis.StringFormat = "£";
            model.Axes.Add(yAxis);

            foreach (string seriesName in seriess.Keys)
            {
                LineSeries series = new LineSeries();
                series.Title = title;
                foreach (double[] t in seriess[seriesName])
                {
                    series.Points.Add(new DataPoint(t[0], t[1]));
                }
                model.Series.Add(series);
            }

            LineAnnotation line = new LineAnnotation();
            line.Type = LineAnnotationType.Horizontal;
            line.Y = 0;
            line.Color = OxyColors.Black;
            line.LineStyle = LineStyle.Solid;
            line.StrokeThickness = 2;

            model.Annotations.Add(line);
            
            plot.Model = model;
            plot.Invalidate();

            Form form = new Form();
            form.Controls.Add(plot);

            form.ShowDialog();
        }
    }
}
