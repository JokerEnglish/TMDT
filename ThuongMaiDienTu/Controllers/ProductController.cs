using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using ThuongMaiDienTu.Repository;
using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.Repository;

namespace ThuongMaiDienTu.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        // 1. Hiển thị form thêm sản phẩm mới (GET)
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // 2. Xử lý dữ liệu form thêm sản phẩm gửi lên (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện và gán đường dẫn vào thuộc tính ImageUrl
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }

            // Nếu dữ liệu không hợp lệ, tải lại danh mục cho form
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        // 1. Hiển thị form cập nhật sản phẩm (GET: Product/Edit/5)
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            // Tạo SelectList và chọn sẵn danh mục hiện tại của sản phẩm (product.CategoryId)
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // 2. Xử lý dữ liệu form cập nhật gửi lên (POST: Product/Edit/5)
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Nếu người dùng tải lên ảnh mới, tiến hành lưu và cập nhật đường dẫn ảnh mới
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                else
                {
                    // Nếu không tải ảnh mới, giữ nguyên ảnh cũ bằng cách lấy lại từ database trung gian
                    var existingProduct = await _productRepository.GetByIdAsync(product.Id);
                    if (existingProduct != null)
                    {
                        product.ImageUrl = existingProduct.ImageUrl;
                        // Nhớ giải phóng thực thể cũ khỏi context để tránh xung đột theo dõi dữ liệu của EF Core
                        _productRepository.DeleteAsync(product.Id); // Hoặc bạn có thể tối ưu logic này tùy theo Repository
                    }
                }

                await _productRepository.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            // Nếu dữ liệu không hợp lệ, tải lại danh sách danh mục cho form
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // Hiển thị thông tin chi tiết sản phẩm (GET: Product/Details/5)
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // 1. Hiển thị form xác nhận xóa sản phẩm (GET: Product/Delete/5)
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // 2. Xử lý thực thi xóa sản phẩm sau khi bấm nút xác nhận (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // 3. Hàm phụ để lưu hình ảnh vào thư mục wwwroot/images
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }
    }
}