using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace yield
{
	class Program
	{
		private readonly Form form;
		private readonly ZedGraphControl chart;
		private volatile bool paused;
		private volatile bool canceled;
		private Thread thread;
		private PointPairList originalPoints;
		private PointPairList expPoints;
		private PointPairList avgPoints;

		public Program()
		{
			form = new Form
			{
				WindowState = FormWindowState.Maximized,
				Text = "Press any key to pause / resume"
			};
			chart = new ZedGraphControl()
			{
				Dock = DockStyle.Fill
			};
			chart.GraphPane.Title.Text = "Сравнение методов сглаживания";
			chart.GraphPane.XAxis.Title.Text = "X";
			chart.GraphPane.YAxis.Title.Text = "Y";
			chart.GraphPane.XAxis.Scale.MaxAuto = true;
			chart.GraphPane.XAxis.Scale.MinAuto = true;
			chart.GraphPane.YAxis.Scale.MaxAuto = true;
			chart.GraphPane.YAxis.Scale.MinAuto = true;
			originalPoints = new PointPairList();
			var original = chart.GraphPane.AddCurve("original", originalPoints, Color.Black, SymbolType.None);
			original.Line.IsAntiAlias = true;
			avgPoints = new PointPairList();
			var avg = chart.GraphPane.AddCurve("avg", avgPoints, Color.Blue, SymbolType.None);
			avg.Line.Width = 3;
			avg.Line.IsAntiAlias = true;
			expPoints = new PointPairList();
			var exp = chart.GraphPane.AddCurve("exp", expPoints, Color.Red, SymbolType.None);
			exp.Line.Width = 3;
			exp.Line.IsAntiAlias = true;
			form.Controls.Add(chart);
			chart.KeyDown += (sender, args) => paused = !paused;
			form.FormClosing += (sender, args) => { canceled = true; };
			form.Shown += OnShown;
		}

		private void OnShown(object sender, EventArgs e)
		{
			thread = new Thread(() =>
			{

				foreach (var p in DataTask.GetData(new Random()))
				{
					if (canceled) return;
					form.BeginInvoke((Action)(() => AddPoint(p)));
					while (paused && !canceled) Thread.Sleep(50);
					Thread.Sleep(50);
				}
			}) { IsBackground = true };
			thread.Start();
		}

		[STAThread]
		static void Main()
		{
			new Program().Run();
		}

		private void Run()
		{
			Application.Run(form);
		}

		private void AddPoint(DataPoint p)
		{
			originalPoints.Add(p.X, p.OriginalY);
			avgPoints.Add(p.X, p.AvgSmoothedY);
			expPoints.Add(p.X, p.ExpSmoothedY);
			chart.AxisChange();
			chart.Invalidate();
			chart.Refresh();
		}
	}
}
