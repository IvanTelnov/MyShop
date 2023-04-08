using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
	public class SignupModel
	{
		[Required]
		[Display(Name = "Почта")]
		public string? Email { get; set; }

		[Required]
		[Display(Name = "Логин")]
		public string? UserName { get; set; }

		[Required]
		[UIHint("Password")]
		[Display(Name = "Пароль")]
		public string? Password { get; set; }
	}
}
