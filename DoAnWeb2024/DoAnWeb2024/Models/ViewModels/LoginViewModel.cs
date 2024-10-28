using System.ComponentModel.DataAnnotations;

namespace DoAnWeb2024.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Hãy nhập Tên người dùng")]
		public string UserName { get; set; }
		
		[DataType(DataType.Password), Required(ErrorMessage = "Hãy nhập mật khẩu")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
