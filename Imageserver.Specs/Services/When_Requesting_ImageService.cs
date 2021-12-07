using Machine.Specifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.IO;
using It = Machine.Specifications.It;
using ItMoq = Moq.It;
using Imageserver.Repositories;
using Imageserver.Services;
using ImageServer.Hubs;
using Imageserver.Utils;

namespace Imageserver.Specs.Services
{
    [Subject(typeof(IImageService))]
    class When_Requesting_Thumbnail_Image
    {
        static IImageService _imageService;
        static string[] imageDir = { "a", "very", "long", "test", "directory", "path", "for", "images" };
        static string[] imagePath = { "a", "very", "long", "test", "directory", "path", "for", "images", "testImage.jpg" };
        static Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        static Mock<IConfigurationSection> _mockSection = new Mock<IConfigurationSection>();
        static Mock<IFileRepository> _fileRepository = new Mock<IFileRepository>();
        static Mock<IHubContext<ImageHub>> _hubContext = new Mock<IHubContext<ImageHub>>();
        static Mock<ILogger<IImageService>> _logger = new Mock<ILogger<IImageService>>();
        static Mock<IImageUtils> _imageUtils = new Mock<IImageUtils>();
        static Stream result;

        Establish context = () =>
        {
            _mockSection.Setup(x => x.Value).Returns("mockValues");
            _configuration.SetupGet(x => x[ItMoq.IsAny<string>()]).Returns(Path.Combine(imageDir));
            _configuration.Setup(x => x.GetSection(ItMoq.IsAny<string>())).Returns(_mockSection.Object);
            _fileRepository.Setup(x => x.GetFilesInDir(ItMoq.IsAny<string>(), ItMoq.IsAny<string[]>())).Returns(new List<string> { Path.Combine(imagePath) });
            _fileRepository.Setup(x => x.CheckIfPathExists(ItMoq.IsAny<string>())).Returns(true);
            _fileRepository.Setup(x => x.GetFileAsStream(ItMoq.IsAny<string>())).Returns(new MemoryStream());
            _imageUtils.Setup(x => x.CreateResizedImage(ItMoq.IsAny<string>(), ItMoq.IsAny<string>(), ItMoq.IsAny<string>(), ItMoq.IsAny<int>(), _fileRepository.Object, _logger.Object)).Returns(0);
            _imageService = new ImageService(_configuration.Object, _fileRepository.Object, _imageUtils.Object, _logger.Object, _hubContext.Object);
        };

        Because of = () => result = _imageService.GetThumbnailImageAsStream("testImage.jpg");

        It should_return_stream = () => result.ShouldBeAssignableTo<Stream>();
    }

    class When_Requesting_Mobile_Image
    {
        static IImageService _imageService;
        static string[] imageDir = { "a", "very", "long", "test", "directory", "path", "for", "images" };
        static string[] imagePath = { "a", "very", "long", "test", "directory", "path", "for", "images", "testImage.jpg" };
        static Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        static Mock<IConfigurationSection> _mockSection = new Mock<IConfigurationSection>();
        static Mock<IFileRepository> _fileRepository = new Mock<IFileRepository>();
        static Mock<IHubContext<ImageHub>> _hubContext = new Mock<IHubContext<ImageHub>>();
        static Mock<ILogger<IImageService>> _logger = new Mock<ILogger<IImageService>>();
        static Mock<IImageUtils> _imageUtils = new Mock<IImageUtils>();
        static Stream result;

        Establish context = () =>
        {
            _mockSection.Setup(x => x.Value).Returns("mockValues");
            _configuration.SetupGet(x => x[ItMoq.IsAny<string>()]).Returns(Path.Combine(imageDir));
            _configuration.Setup(x => x.GetSection(ItMoq.IsAny<string>())).Returns(_mockSection.Object);
            _fileRepository.Setup(x => x.GetFilesInDir(ItMoq.IsAny<string>(), ItMoq.IsAny<string[]>())).Returns(new List<string> { Path.Combine(imagePath) });
            _fileRepository.Setup(x => x.CheckIfPathExists(ItMoq.IsAny<string>())).Returns(true);
            _fileRepository.Setup(x => x.GetFileAsStream(ItMoq.IsAny<string>())).Returns(new MemoryStream());
            _imageUtils.Setup(x => x.CreateResizedImage(ItMoq.IsAny<string>(), ItMoq.IsAny<string>(), ItMoq.IsAny<string>(), ItMoq.IsAny<int>(), _fileRepository.Object, _logger.Object)).Returns(0);
            _imageService = new ImageService(_configuration.Object, _fileRepository.Object, _imageUtils.Object, _logger.Object, _hubContext.Object);
        };

        Because of = () => result = _imageService.GetMobileImageAsStream("testImage.jpg");

        It should_return_stream = () => result.ShouldBeAssignableTo<Stream>();
    }

    class When_Requesting_FullRes_Image
    {
        static IImageService _imageService;
        static string[] imageDir = { "a", "very", "long", "test", "directory", "path", "for", "images" };
        static string[] imagePath = { "a", "very", "long", "test", "directory", "path", "for", "images", "testImage.jpg" };
        static Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        static Mock<IConfigurationSection> _mockSection = new Mock<IConfigurationSection>();
        static Mock<IFileRepository> _fileRepository = new Mock<IFileRepository>();
        static Mock<IHubContext<ImageHub>> _hubContext = new Mock<IHubContext<ImageHub>>();
        static Mock<ILogger<IImageService>> _logger = new Mock<ILogger<IImageService>>();
        static Mock<IImageUtils> _imageUtils = new Mock<IImageUtils>();
        static Stream result;

        Establish context = () =>
        {
            _mockSection.Setup(x => x.Value).Returns("mockValues");
            _configuration.SetupGet(x => x[ItMoq.IsAny<string>()]).Returns(Path.Combine(imageDir));
            _configuration.Setup(x => x.GetSection(ItMoq.IsAny<string>())).Returns(_mockSection.Object);
            _fileRepository.Setup(x => x.GetFilesInDir(ItMoq.IsAny<string>(), ItMoq.IsAny<string[]>())).Returns(new List<string> { Path.Combine(imagePath) });
            _fileRepository.Setup(x => x.CheckIfPathExists(ItMoq.IsAny<string>())).Returns(true);
            _fileRepository.Setup(x => x.GetFileAsStream(ItMoq.IsAny<string>())).Returns(new MemoryStream());
            _imageUtils.Setup(x => x.CreateResizedImage(ItMoq.IsAny<string>(), ItMoq.IsAny<string>(), ItMoq.IsAny<string>(), ItMoq.IsAny<int>(), _fileRepository.Object, _logger.Object)).Returns(0);
            _imageService = new ImageService(_configuration.Object, _fileRepository.Object, _imageUtils.Object, _logger.Object, _hubContext.Object);
        };

        Because of = () => result = _imageService.GetImageAsStream("testImage.jpg");

        It should_return_stream = () => result.ShouldBeAssignableTo<Stream>();
    }
}
