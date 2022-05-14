using Core.Entities;

namespace ShopperAPi.Helpers
{
    public interface IImageHandler
    {
        
        public string UploadImage(Product product);
        public void RemoveImage(string imgPath);
    }
}
