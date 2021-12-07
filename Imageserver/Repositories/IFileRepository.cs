namespace Imageserver.Repositories
{
    public interface IFileRepository
    {
        IEnumerable<string> GetFilesInDir(string path, string[] fileExtensions);
        Stream GetFileAsStream(string path);
        void CreateDirectory(string path);
        void DeletePath(string path);
        bool CheckIfPathExists(string path);
    }
}