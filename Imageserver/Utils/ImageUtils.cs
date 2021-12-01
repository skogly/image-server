namespace Imageserver.Utils
{
    public static class ImageUtils
    {
        public static int CreateResizedImage(string path, string imageFolder, string saveFolder, int maxPixels, IFileRepository fileRepository, ILogger logger)
        {
            var savePath = "";

            try
            {
                savePath = path.Replace(imageFolder, saveFolder);
                fileRepository.CreateDirectory(savePath);
            }
            catch (Exception e)
            {
                logger.LogError($"Could not create new folder: {Path.GetDirectoryName(savePath)}");
                logger.LogError(e.Message);
                return 1;
            }

            try
            {
                if (!File.Exists(savePath))
                {
                    using (FileStream pngStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    using (var image = new Bitmap(pngStream))
                    {
                        if (image.PropertyIdList.Contains(0x112))
                        {
                            try
                            {
                                var orientation = (int)image.GetPropertyItem(0x112).Value[0];

                                if (orientation == 8)
                                {
                                    image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                }
                                else if (orientation == 3)
                                {
                                    image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                }
                                else if (orientation == 6)
                                {
                                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                }
                            }
                            catch (Exception)
                            {
                                logger.LogError("Was not able to rotate image");
                            }
                        }
                        var newSize = CalculateWidthAndHeight(image.Width, image.Height, maxPixels);
                        var resized = new Bitmap(newSize.width, newSize.height);
                        using (var graphics = Graphics.FromImage(resized))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.DrawImage(image, 0, 0, newSize.width, newSize.height);
                            var fileExt = Path.GetExtension(path);
                            if (fileExt == ".png" || fileExt == ".Png" || fileExt == ".PNG")
                            {
                                resized.Save(savePath, ImageFormat.Png);
                            }
                            else
                            {
                                resized.Save(savePath, ImageFormat.Jpeg);
                            }
                        }
                        resized.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Could not save resized image: {savePath}");
                logger.LogError(e.Message);
                return 1;
            }
            return 0;
        }

        private static (int width, int height) CalculateWidthAndHeight(int imageWidth, int imageHeight, int maxPixels)
        {
            double width, height;
            if (imageWidth > imageHeight)
            {
                var factor = (double)maxPixels / imageWidth;
                width = imageWidth * factor;
                height = imageHeight * factor;
            }
            else
            {
                var factor = (double)maxPixels / imageHeight;
                width = imageWidth * factor;
                height = imageHeight * factor;
            }
            return ((int)width, (int)height);
        }
    }
}
