using DoAnWeb2024.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWeb2024.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Nhập tên sản phẩm")]
        public string Name { get; set; }

        [Required,MinLength(4,ErrorMessage ="Nhập mô tả sản phẩm")]
        public string Description { get; set; }

        public string Slug { get; set; }
        [Required(ErrorMessage ="Nhập giá sản phẩm")]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]

        public decimal Price { get; set; }
        [Required,Range(1,int.MaxValue,ErrorMessage = "Chọn 1 thương hiệu")]

        public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn 1 loại sản phẩm")]
        public int CategoryId { get;set; }
        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }

        public string Image { get; set; } = "noimage.jpg";
        [NotMapped] // o lquan đến db
        [FileExtension] //ktra đuôi 
        
        public IFormFile ImageUpload { get; set; }
    }
}
