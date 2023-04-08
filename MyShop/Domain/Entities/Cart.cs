namespace MyShop.Domain.Entities
{
    public class Cart
    {
        public List<CartItem>? Items { get; set; }

        public Cart(List<CartItem> items)
        {
            Items = items;
        }
    }

    public class CartItem
    {
        public Guid CarId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
