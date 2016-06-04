using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace yield
{
    public class FileData
    {
        public string FileExtension;
        public long Size;
    }

    public static class DirectoriesTask
    {
        public static IEnumerable<FileData> EnumerateAllFiles(DirectoryInfo directoryInfo, Random random)
        {
            var dfsOrder = GetDirectories(directoryInfo)
                .Shuffle(random)
                .SelectMany(dir => EnumerateAllFiles(dir, random));
            foreach (var fileData in dfsOrder)
                yield return fileData;

            var currentPathFiles = GetFiles(directoryInfo).Shuffle(random);
            foreach (var file in currentPathFiles)
                yield return new FileData {FileExtension = file.Extension, Size = TryGetLength(file)};
        }

        private static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items, Random random)
        {
            return items.OrderBy(item => random.Next());
        }

        private static long TryGetLength(FileInfo file)
        {
            return file.Attributes.HasFlag(FileAttributes.System)
                ? 0
                : file.Length;
        }

        private static FileInfo[] GetFiles(DirectoryInfo directoryInfo)
        {
            return directoryInfo.Attributes.HasFlag(FileAttributes.System)
                ? new FileInfo[0]
                : directoryInfo.GetFiles();
        }

        private static DirectoryInfo[] GetDirectories(DirectoryInfo directoryInfo)
        {
            return directoryInfo.Attributes.HasFlag(FileAttributes.System)
                ? new DirectoryInfo[0]
                : directoryInfo.GetDirectories();
        }
    }
}
