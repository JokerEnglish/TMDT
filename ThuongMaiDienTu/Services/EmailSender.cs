using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace ThuongMaiDienTu.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Trả về Task hoàn thành ngay lập tức
            // Hệ thống sẽ không báo lỗi nữa vì nó tưởng là đã gửi mail thành công
            return Task.CompletedTask;
        }
    }
}