using Assignment_Elhoseen_API.Data;
using Assignment_Elhoseen_API.DTOs;
using Assignment_Elhoseen_API.Models;
using EcommerceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public ProductService(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<Product> CreateAsync(ProductCreateDto dto)
        {
            if (await _context.Products.AnyAsync(p => p.Code == dto.Code))
                throw new Exception("Product code already exists.");

            var product = new Product
            {
                Category = dto.Category,
                Code = dto.Code,
                Name = dto.Name,
                Price = dto.Price,
                MinQuantity = dto.MinQuantity,
                DiscountRate = dto.DiscountRate
            };

            if (dto.Image != null)
                product.ImageUrl = await _imageService.UploadImageAsync(dto.Image, "products");

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return ToResponseDto(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(p => ToResponseDto(p));
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? null : ToResponseDto(product);
        }

        public async Task<Product?> UpdateAsync(int id, ProductUpdateDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            product.Category = dto.Category;
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.MinQuantity = dto.MinQuantity;
            product.DiscountRate = dto.DiscountRate;

            if (dto.Image != null)
                product.ImageUrl = await _imageService.UploadImageAsync(dto.Image, "products");

            await _context.SaveChangesAsync();
            return ToResponseDto(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        private Product ToResponseDto(Product product)
        {
            return new Product
            {
                Id = product.Id,
                Category = product.Category,
                Code = product.Code,
                Name = product.Name,
                ImageUrl = string.IsNullOrEmpty(product.ImageUrl) ? "" : $"/{product.ImageUrl}",
                Price = product.Price,
                MinQuantity = product.MinQuantity,
                DiscountRate = product.DiscountRate
            };
        }
    }
}
