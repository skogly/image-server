namespace ImageServer.Controllers
{
    [Route("/")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("images")]
        public IList<string> GetImageFiles()
        {
            var imageFiles = _imageService.GetImageFiles();
            return imageFiles;
        }

        [HttpGet("updateImages")]
        public ActionResult UpdateImageFiles()
        {
            _imageService.UpdateImages();
            return Ok();
        }

        [HttpPost("getThumbnailImage")]
        public ActionResult GetThumbnailImageByPath([FromBody] JsonRequest jsonRequest)
        {
            string path = jsonRequest.Path;
            var image = _imageService.GetThumbnailImageAsStream(path);
            if (image is not null) return File(image, "image/jpeg");
            return Ok();
        }

        [HttpPost("getMobileImage")]
        public ActionResult GetMedResImageByPath([FromBody] JsonRequest jsonRequest)
        {
            string path = jsonRequest.Path;
            var image = _imageService.GetMobileImageAsStream(path);
            if (image is not null) return File(image, "image/jpeg");
            return Ok();
        }

        [HttpPost("getFullResImage")]
        public ActionResult GetHiResImageByPath([FromBody] JsonRequest jsonRequest)
        {
            string path = jsonRequest.Path;
            var image = _imageService.GetImageAsStream(path);
            if (image is not null) return File(image, "image/jpeg");
            return Ok();
        }

        [HttpPost("uploadImage")]
        public async Task<ActionResult> UploadImage([FromForm] ImageRequest imageRequest)
        {
            await _imageService.SaveImage(imageRequest);
            return Ok();
        }

        [HttpPost("deleteImage")]
        public async Task<ActionResult> DeleteImage([FromBody] JsonRequest jsonRequest)
        {
            string path = jsonRequest.Path;
            await _imageService.DeleteImage(path);
            return Ok();
        }
    }
}