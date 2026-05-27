using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ThuongMaiDienTu.Services;
using Microsoft.EntityFrameworkCore;
using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.Repository; // Đảm bảo bạn có dòng này

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Cấu hình Identity CHUẨN
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    // Dòng này giúp bỏ qua việc cần Email xác thực
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. Đăng ký Repository (Giải quyết lỗi IProductRepository)
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

// 4. Dịch vụ khác
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddTransient<IEmailSender, EmailSender>();
var app = builder.Build();

// 5. Middleware pipeline
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // BẮT BUỘC TRƯỚC
app.UseAuthorization();  // NÀY

app.MapControllerRoute(name: "default", pattern: "{controller=Product}/{action=Index}/{id?}");
app.MapRazorPages(); // BẮT BUỘC ĐỂ LOAD TRANG IDENTITY

app.Run();