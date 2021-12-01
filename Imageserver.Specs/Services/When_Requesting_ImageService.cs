using Machine.Specifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using It = Machine.Specifications.It;
using ItMoq = Moq.It;
using Imageserver.Repositories;
using Imageserver.Services;
using ImageServer.Hubs;

namespace Imageserver.Specs.Services
{
    [Subject(typeof(IImageService))]
    class When_Requesting_Thumbnail_Image
    {
        static IImageService _imageService;
        static Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        static Mock<IConfigurationSection> _mockSection = new Mock<IConfigurationSection>();
        static Mock<IFileRepository> _fileRepository = new Mock<IFileRepository>();
        static Mock<IHubContext<ImageHub>> _hubContext = new Mock<IHubContext<ImageHub>>();
        static Mock<ILogger<IImageService>> _logger = new Mock<ILogger<IImageService>>();
        static Stream result;

        Establish context = () =>
        {
            _mockSection.Setup(x => x.Value).Returns("testValue");
            _configuration.Setup(x => x.GetSection(ItMoq.IsAny<string>())).Returns(_mockSection.Object);
            _imageService = new ImageService(_configuration.Object, _fileRepository.Object, _logger.Object, _hubContext.Object);
        };

        Because of = () => result = _imageService.GetThumbnailImageAsStream("path");

        // Should return null since we do not have access to real image files
        It should_return_null = () => result.ShouldBeNull();
    }

    class When_Requesting_Mobile_Image
    {
        static IImageService _imageService;
        static Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        static Mock<IConfigurationSection> _mockSection = new Mock<IConfigurationSection>();
        static Mock<IFileRepository> _fileRepository = new Mock<IFileRepository>();
        static Mock<IHubContext<ImageHub>> _hubContext = new Mock<IHubContext<ImageHub>>();
        static Mock<ILogger<IImageService>> _logger = new Mock<ILogger<IImageService>>();
        static Stream result;

        Establish context = () =>
        {
            _mockSection.Setup(x => x.Value).Returns("testValue");
            _configuration.Setup(x => x.GetSection(ItMoq.IsAny<string>())).Returns(_mockSection.Object);
            _imageService = new ImageService(_configuration.Object, _fileRepository.Object, _logger.Object, _hubContext.Object);
        };

        Because of = () => result = _imageService.GetMobileImageAsStream("path");

        // Should return null since we do not have access to real image files
        It should_return_null = () => result.ShouldBeNull();

    }

    class When_Requesting_FullRes_Image
    {
        static IImageService _imageService;
        static Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        static Mock<IConfigurationSection> _mockSection = new Mock<IConfigurationSection>();
        static Mock<IFileRepository> _fileRepository = new Mock<IFileRepository>();
        static Mock<IHubContext<ImageHub>> _hubContext = new Mock<IHubContext<ImageHub>>();
        static Mock<ILogger<IImageService>> _logger = new Mock<ILogger<IImageService>>();
        static Stream result;

        Establish context = () =>
        {
            _mockSection.Setup(x => x.Value).Returns("testValue");
            _configuration.Setup(x => x.GetSection(ItMoq.IsAny<string>())).Returns(_mockSection.Object);
            _imageService = new ImageService(_configuration.Object, _fileRepository.Object, _logger.Object, _hubContext.Object);
        };

        Because of = () => result = _imageService.GetImageAsStream("path");

        // Should return null since we do not have access to real image files
        It should_return_null = () => result.ShouldBeNull();

    }
}
