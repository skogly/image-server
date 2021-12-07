
namespace Imageserver.Utils
{
    public interface IImageUtils
    {
        int CreateResizedImage(string path, string imageFolder, string saveFolder, int maxPixels, IFileRepository fileRepository, ILogger logger);
    }
}