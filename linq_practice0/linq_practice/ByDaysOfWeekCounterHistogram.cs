using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace linq_practice
{
    public partial class ByDaysOfWeekCounterHistogram : Form
    {
        private const int WindowWidth = 400;
        private const int WindowHeight = 300;
        private const int DaysInWeek = 7;
        private readonly int[] counter;
        private Chart chart;

        public ByDaysOfWeekCounterHistogram(IEnumerable<Visit> visits)
        {
            counter = new int[DaysInWeek];
            foreach (var dayOfWeek in visits.Select(visit => visit.Date.DayOfWeek))
                counter[(int) dayOfWeek]++;
//            counter = visits.GroupBy(x => x.Date.DayOfWeek)
//                .OrderBy(x => x.Key)
//                .Select(x => x.Count())
//                .ToArray();

            InitializeComponent();
            ClientSize = new Size(WindowWidth, WindowHeight);
        }

        private static readonly string[] DaysOfWeek =
            Enum.GetValues(typeof (DayOfWeek)).Cast<DayOfWeek>().Select(day => day.ToString()).ToArray();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            chart = new Chart
            {
                Size = new Size(ClientSize.Width, ClientSize.Height)
            };
            chart.Titles.Add(
                new Title("Count of visits by day of week", Docking.Top,
                    new Font(FontFamily.GenericSerif, 20), Color.DeepSkyBlue));
            
            var area = chart.ChartAreas.Add("area");
            area.AxisY.Interval = 500;
            area.AxisY.MajorGrid.LineColor = Color.Red;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.BackColor = Color.Black;

            var series = chart.Series.Add("series");
            series.XValueType = ChartValueType.String;
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.LightGreen;

            for (var dayOfWeek = 0; dayOfWeek < DaysInWeek; dayOfWeek++)
                series.Points.Add(new DataPoint(dayOfWeek, counter[dayOfWeek])
                {
                    AxisLabel = DaysOfWeek[dayOfWeek]
                });

            Controls.Add(chart);
        }
    }
}
