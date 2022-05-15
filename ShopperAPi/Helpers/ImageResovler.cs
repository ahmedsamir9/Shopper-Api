using AutoMapper;
using Core.Entities;
using ShopperAPi.DTOS.ProductDTOs;

namespace ShopperAPi.Helpers
{

    public class ImageResovler : IValueResolver<ProductMainpulationsDto, Product, string>
    {
        private readonly IImageHandler _imageHandler;
        public ImageResovler(IImageHandler imageHandler)
        {
            _imageHandler = imageHandler;

        }
        public string Resolve(ProductMainpulationsDto source, Product destination, string destMember, ResolutionContext context)
        {
            return _imageHandler.UploadImage(source.ImageFile);
        }
    }

}
