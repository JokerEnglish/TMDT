using System.Collections.Generic;
using System.Threading.Tasks;
using ThuongMaiDienTu.Models; // Dòng này giúp nhận diện các Model như Product

namespace ThuongMaiDienTu.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}