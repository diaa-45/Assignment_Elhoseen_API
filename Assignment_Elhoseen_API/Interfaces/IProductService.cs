using Assignment_Elhoseen_API.DTOs;
using Assignment_Elhoseen_API.Models;
namespace EcommerceAPI.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateAsync(ProductCreateDto dto);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> UpdateAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
