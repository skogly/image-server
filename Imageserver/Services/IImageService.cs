namespace Imageserver.Services
{
    public interface IImageService
    {
        IList<string> GetImageFiles();
        void UpdateImages();
        Stream GetThumbnailImageAsStream(string path);
        Stream GetMobileImageAsStream(string path);
        Stream GetImageAsStream(string path);
        Task SaveImage(ImageRequest imageRequest);
        Task DeleteImage(string path);
    }
}
