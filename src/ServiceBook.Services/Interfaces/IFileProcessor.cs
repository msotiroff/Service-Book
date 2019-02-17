using System.Threading.Tasks;

namespace ServiceBook.Services.Interfaces
{
    public interface IFileProcessor
    {
        Task CopyAsync(string sourcePath, string destinationPath, bool overwrite = false);
    }
}
