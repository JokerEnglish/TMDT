using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThuongMaiDienTu.Models; // Để nhận diện Product và ApplicationDbContext

namespace ThuongMaiDienTu.Repository
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor: Nhận bối cảnh dữ liệu được truyền vào (Dependency Injection)
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy toàn bộ danh sách sản phẩm kèm theo danh mục của nó
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        // Tìm một sản phẩm theo ID cụ thể
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Thêm sản phẩm mới vào DB
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin sản phẩm
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Xóa sản phẩm dựa vào ID
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}