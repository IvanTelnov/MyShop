namespace MyShop.Domain.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public string? Sender { get; set; }
		public decimal Sum { get; set; }
		public string? OperationId { get; set; }
		public decimal? Amount { get; set; }
		public decimal? WithdrawAmount { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public Guid UserId { get; set; }

	}
}
