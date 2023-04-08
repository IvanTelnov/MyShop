namespace MyShop.Domain.Entities
{
	public class CartDb
	{
		public Guid Id { get; set; }
		public string? UserId { get; set; }
		public string? SerializedCart { get; set; }
    }
}
