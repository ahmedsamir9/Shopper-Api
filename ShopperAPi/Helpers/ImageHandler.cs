using Core.Entities;
using System;
using System.IO;

namespace ShopperAPi.Helpers
{
    public class ImageHandler : IImageHandler
    {
        public ImageHandler(IWebHostEnvironment webhost, IConfiguration config)
        {
            Webhost = webhost;
            this.config = config;
        }

        private readonly IWebHostEnvironment Webhost;
        private readonly IConfiguration config;

        public void RemoveImage(string imgPath)
        {
            ///remove the url
            string[] words = imgPath.Split("images/");
            imgPath = words[1];

            string image = Path.Combine(Webhost.WebRootPath, "images", imgPath);
            FileInfo fi = new FileInfo(image);
            if (fi != null)
            {
                System.IO.File.Delete(image);
                fi.Delete();
            }
        }

        public string UploadImage(Product product)
        {
            string uniqueFileName = null;

            if (product.ImageFile != null)
            {

                string uploadsFolder = Path.Combine(Webhost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.ImageFile.CopyTo(fileStream);

                }

                uniqueFileName = $"{config["ApiUrl"]}images/{uniqueFileName}";

            }

            return uniqueFileName;
        }
    }
}
