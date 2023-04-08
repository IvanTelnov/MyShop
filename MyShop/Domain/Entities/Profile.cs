using Microsoft.AspNetCore.Identity;
using MyShop.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Domain.Entities
{
	public class Profile
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		[Display(Name="Имя")]
		[StringLength(30, MinimumLength = 4, ErrorMessage ="Длина должна быть от 3 до 30")]
		[RegularExpression(@"[A-Za-zА-Яа-я0-9] + [A-Za-zА-Яа-я0-9]", ErrorMessage ="Некорректное имя пользователя")]
		public string? UserName { get; set; }

		public string? Email { get; set; }

		[Required]
		[Display(Name ="Дата Рождения")]
		[DataType(DataType.Date)]
		public DateTime BirthDate { get; set; }

		[Required]
		[Display(Name ="Пол")]
		public string? Sex { get; set; }

		[Display(Name = "Фото профиля")]
		public string? ImagePath { get; set; }

	}
}
