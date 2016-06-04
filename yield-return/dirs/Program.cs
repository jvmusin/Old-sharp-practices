using System;
using System.IO;
using System.Linq;

namespace yield
{
	class Program
	{
		static void Main()
		{
		    for (int i = 0; i < 30; i++)
		    {
		        var folderToAnalyze = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		        RunExtensionsSizeTask(folderToAnalyze, 10000);
		    }
            Console.WriteLine("DONE");
		}

		private static void RunExtensionsSizeTask(string path, int filesToAnalyze)
		{
			Console.WriteLine("Analyzing {1} files from '{0}' ...", path, filesToAnalyze);
			Console.WriteLine();
			var fileExtensionsRating =
				DirectoriesTask.EnumerateAllFiles(new DirectoryInfo(path), new Random())
					.Take(filesToAnalyze)
					.GroupBy(t => t.FileExtension.ToLower(), t => t.Size, (ext, g) => new { ext, Size = g.Sum() })
					.OrderByDescending(t => t.Size);

			Console.WriteLine("Mb\tFile extension");
			foreach (var extData in fileExtensionsRating.Take(10))
				Console.WriteLine("{0}\t{1}", extData.Size / 1024 / 1024, extData.ext);
			Console.WriteLine("...");
		}
	}
}
