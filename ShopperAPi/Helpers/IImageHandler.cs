using Core.Entities;

namespace ShopperAPi.Helpers
{
    public interface IImageHandler
    {
        
        public string UploadImage(IFormFile file);
        public void RemoveImage(string imgPath);
    }
}
