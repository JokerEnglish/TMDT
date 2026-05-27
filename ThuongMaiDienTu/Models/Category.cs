using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ThuongMaiDienTu.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [StringLength(50)]
        public string Name { get; set; }

        public List<Product>? Products { get; set; }
    }
}