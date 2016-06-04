using System;
using System.Windows.Forms;

namespace func_rocket
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
		    var form = new GameForm(Tasks.GetGameSpaces()) {Autopilot = Tasks.TurboDoodleTechnique};
		    Application.Run(form);
		}
	}
}
