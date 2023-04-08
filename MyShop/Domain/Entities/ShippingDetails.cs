using System.ComponentModel.DataAnnotations;

namespace MyShop.Domain.Entities
{
    public class ShippingDetails
    {

        [Required(ErrorMessage = "Укажите имя получателя")]
        [Display(Name = "Имя получателя")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Укажите страну")]
        [Display(Name = "Страна")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Укажите город или другой населенный пункт")]
        [Display(Name = "Город/населенный пункт")]
        public string? Locality { get; set; }

        [Required(ErrorMessage = "Укажите улицу")]
        [Display(Name = "Улица")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Укажите дом")]
        [Display(Name ="Дом")]
        public string? Home { get; set; }
    }
}
