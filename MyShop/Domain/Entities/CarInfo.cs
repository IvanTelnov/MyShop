using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace MyShop.Domain.Entities
{
	public class CarInfo
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		[Display(Name = "Название машины")]
		public string? Name { get; set; }

		[Required]
		[Display(Name = "Краткое описание")]
		public virtual string? ShortDescription { get; set; }

		[Display(Name = "Полное описание")]
		public virtual string? FullDescription { get; set; }

		[Display(Name = "Картинка")]
		public string? ImagePath { get; set; }

		[Required]
		[Display(Name = "Категория машины")]
		public string? Category { get; set; }

		[Required]
		[Display(Name = "Цена")]
		public int Price { get; set; }

		public int Quantity { get; set; }

	}
}
