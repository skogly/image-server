namespace Imageserver.Repositories
{
    public class FileRepository : IFileRepository
    {
        public IEnumerable<string> GetFilesInDir(string path, string[] fileExtensions)
        {
            fileExtensions = fileExtensions.Select(x => "*." + x).ToArray();
            var files = fileExtensions.SelectMany(x => Directory.EnumerateFiles(path, x, SearchOption.AllDirectories));
            return files;
        }

        public Stream GetFileAsStream(string path)
        {
            return File.OpenRead(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        public void DeletePath(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}
