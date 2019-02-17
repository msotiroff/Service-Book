using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ServiceBook.Services.Interfaces;

namespace ServiceBook.Services
{
    public class FileProcessor : IFileProcessor
    {
        public async Task CopyAsync(
            string sourcePath, string destinationPath, bool overwrite = false)
        {
            var bufferSize = 1024 * 1024;

            using (var destFileStream = new FileStream(
                destinationPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var srcFileStream = new FileStream(
                    sourcePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    destFileStream.SetLength(srcFileStream.Length);
                    var bytesRead = -1;
                    var bytes = new byte[bufferSize];

                    while ((bytesRead = await srcFileStream.ReadAsync(bytes, 0, bufferSize)) > 0)
                    {
                        await destFileStream.WriteAsync(bytes, 0, bytesRead);
                    }
                }
            }
        }

        public async Task OpenAsync(string filePath)
        {
            await Task.Run(() =>
            {
                using (var process = new Process())
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = filePath;
                    process.Start();
                }
            });
        }
    }
}
