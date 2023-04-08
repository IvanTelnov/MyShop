using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
	public class LoginModel
	{
		[Required]
		[Display(Name = "Логин")]
		public string? UserName { get; set; }

		[Required]
		[UIHint("Password")]
		[Display(Name = "Пароль")]
		public string? Password { get; set; }

		[Display(Name = "Запомнить меня?")]
		public bool RememberMe { get; set; }

	}
}
