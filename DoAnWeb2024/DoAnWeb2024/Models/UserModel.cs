using System.ComponentModel.DataAnnotations;

namespace DoAnWeb2024.Models
{
	public class UserModel
	{
		public int Id { get;set; }
		[Required(ErrorMessage ="Hãy nhập Tên người dùng")]
		public string UserName { get;set; }
		[Required(ErrorMessage = "Hãy nhập Email"),EmailAddress]

		public string Email { get;set; }
		[DataType(DataType.Password),Required(ErrorMessage ="Hãy nhập mật khẩu")]
		public string Password { get;set; }
	}
}
