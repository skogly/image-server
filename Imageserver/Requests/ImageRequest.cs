namespace Imageserver.Requests
{
    public class ImageRequest
    {
        public string FilePath { get; set; }
        public IFormFile ImageFile { get; set; }
    }
    public static class Extension
    {
        public static async Task SaveAsAsync(this IFormFile formFile, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
        }
    }

}
