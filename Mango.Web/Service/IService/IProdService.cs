using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProdService
    {
        Task<ResponseDTO?> GetAllProducts();
        Task<ResponseDTO?> GetProductsById(int id);
        Task<ResponseDTO?> GetProductsByName(string name);
        Task<ResponseDTO?> CreateProduct(ProductDTO productDTO);
        Task<ResponseDTO?> UpdateProduct(ProductDTO productDTO);
        Task<ResponseDTO?> DeleteProduct(int id);
    }
}
