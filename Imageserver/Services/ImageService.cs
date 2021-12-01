namespace Imageserver.Services
{
    public class ImageService: IImageService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IHubContext<ImageHub> _hubContext;
        private readonly ILogger<IImageService> _logger;
        private static IList<string> _imageFiles;
        private static int imagesProcessed = 0;
        private readonly string imageDir;
        private readonly string mobileDir;
        private readonly string thumbnailDir;
        private readonly string[] imageExtensions;

        public ImageService(IConfiguration configuration, IFileRepository fileRepository,
                            ILogger<IImageService> logger, IHubContext<ImageHub> hubContext)
        {
            _fileRepository = fileRepository;
            _hubContext = hubContext;
            _logger = logger;
            imageDir = configuration["ImageFolder"];
            mobileDir = configuration["MobileFolder"];
            thumbnailDir = configuration["ThumbnailFolder"];
            imageExtensions = configuration.GetSection("ImageExtensions").Get<string[]>();
            var errors = 0;
            errors += TryCreateFolders(mobileDir);
            errors += TryCreateFolders(thumbnailDir);
            if (HasNoErrors(errors))
            {
                FindAllImages();
            }
        }

        public IList<string> GetImageFiles()
        {
            return _imageFiles;
        }
        
        public Stream GetThumbnailImageAsStream(string path)
        {
            if (_imageFiles.Contains(path))
            {
                return _fileRepository.GetFileAsStream(Path.Combine(thumbnailDir, path));
            }
            return null;
        }

        public Stream GetMobileImageAsStream(string path)
        {
            if (_imageFiles.Contains(path))
            {
                return _fileRepository.GetFileAsStream(Path.Combine(mobileDir, path));
            }
            return null;
        }
        public Stream GetImageAsStream(string path)
        {
            if (_imageFiles.Contains(path))
            {
                return _fileRepository.GetFileAsStream(Path.Combine(imageDir, path));
            }
            return null;
        }

        public void UpdateImages()
        {
            FindAllImages();
        }

        public async Task SaveImage(ImageRequest imageRequest)
        {
            if ((imageRequest?.FilePath == null) || (imageRequest?.ImageFile == null))
            {
                _logger.LogError($"Error saving image. Wrong body request: {imageRequest}");
                return;
            }

            string path = Path.Combine(imageDir, imageRequest.FilePath);
            IFormFile imageFile = imageRequest.ImageFile;
            
            _fileRepository.CreateDirectory(path);
            await imageFile.SaveAsAsync(path);

            FindAllImages();

            await _hubContext.Clients.All.SendAsync("Notify", "newImages");
        }

        public async Task DeleteImage(string path)
        {
            if (_imageFiles.Contains(path) == false)
            {
                _logger.LogError($"Error deleting image. Image path is not a known path. Path: {path}");
                return;
            }
            var directories = new List<string>() { imageDir, mobileDir, thumbnailDir };

            foreach (string directory in directories)
            {
                var pathToDelete = Path.Combine(directory, path);
                _fileRepository.DeletePath(pathToDelete);
            }

            FindAllImages();

            await _hubContext.Clients.All.SendAsync("Notify", "deletedImages");
        }

        private bool HasNoErrors(int errors)
        {
            return errors == 0;
        }

        private int TryCreateFolders(string folder)
        {
            try
            {
                _fileRepository.CreateDirectory(folder);
                return 0;
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not create new folder: {folder}");
                _logger.LogError(e.Message);
                return 1;
            }
        }

        private void FindAllImages()
        {
            _logger.LogInformation($"Going through images in directory {imageDir} at {DateTime.Now.ToString("HH:mm:ss")}");
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            imagesProcessed = 0;
            _imageFiles = new List<string>();
            
            IEnumerable<string> imageFiles = _fileRepository.GetFilesInDir(imageDir, imageExtensions);
            
            Parallel.ForEach(imageFiles, imageFile =>
            {
                CreateResizedImages(imageFile);
            });

            stopWatch.Stop();
            
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

            _logger.LogInformation($"Total {imagesProcessed} images processed in {elapsedTime} (min:seconds)");
        }

        private void CreateResizedImages(string path)
        {
            var errors = 0;
            errors += ImageUtils.CreateResizedImage(path, imageDir, thumbnailDir, maxPixels: 200, _fileRepository, _logger);
            errors += ImageUtils.CreateResizedImage(path, imageDir, mobileDir, maxPixels: 1000, _fileRepository, _logger);

            if (HasNoErrors(errors))
            {
                _imageFiles.Add(GetImagePath(path));
                imagesProcessed++;
                if (imagesProcessed % 100 == 0)
                {
                    _logger.LogInformation($"{imagesProcessed} images processed at {DateTime.Now.ToString("HH:mm:ss")}");
                }
            }
        }

        private string GetImagePath(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var tempPath = string.Join("\\", path.Split("\\").Skip(2).ToList());
                if (File.Exists(Path.Combine(imageDir, tempPath))) {
                    return string.Join("\\", path.Split("\\").Skip(2).ToList());
                } else
                {
                    return null;
                }
            }
            else
            {
                var tempPath = string.Join("/", path.Split("/").Skip(3).ToList());
                if (File.Exists(Path.Combine(imageDir, tempPath)))
                {
                    return string.Join("/", path.Split("/").Skip(3).ToList());
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
