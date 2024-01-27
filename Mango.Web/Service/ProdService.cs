using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProdService : IProdService
    {
        private readonly IBaseService _baseService;
        public ProdService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> CreateProduct(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.POST,
                Data = productDTO,
                URL = StaticDetails.ProductAPIBase + "/api/ProductAPI/"
            });
        }

        public async Task<ResponseDTO?> DeleteProduct(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType= StaticDetails.APIType.DELETE,
                URL = StaticDetails.ProductAPIBase + "/api/ProductAPI/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllProducts()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.GET,
                URL = StaticDetails.ProductAPIBase + "/api/ProductAPI/"
            });
        }

        public async Task<ResponseDTO?> GetProductsById(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.GET,
                URL = StaticDetails.ProductAPIBase + "/api/ProductAPI/" + id,
            });
        }

        public async Task<ResponseDTO?> GetProductsByName(string name)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.GET,
                URL = StaticDetails.ProductAPIBase + "/api/ProductAPI/GetByName/" + name,
            });
        }

        public async Task<ResponseDTO?> UpdateProduct(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.PUT,
                Data = productDTO,
                URL = StaticDetails.ProductAPIBase + "/api/ProductAPI/"
            });
        }
    }
}
