using Microsoft.EntityFrameworkCore;
using MyShop.Domain;
using MyShop.Domain.Entities;
using System.Text.Json;

namespace MyShop.Services
{
    public static class CartDbProccesing
    {
        public static Cart? GetCartFromDb(AppDbContext context, string? userId)
        {
            CartDb? cart = context.Carts.SingleOrDefault(c => c.UserId == userId);
            string? serializedCart = cart?.SerializedCart;

            if(serializedCart != null)
            {
                Cart? deserializedCart = JsonSerializer.Deserialize<Cart>(serializedCart);
                return deserializedCart;
            }
            return null;
        }

        public static void SetCartToDb(AppDbContext context, Cart? deserializedCart, string? userId)
        {
            CartDb? cart = context.Carts.SingleOrDefault(c => c.UserId == userId);
            string serializedCart = JsonSerializer.Serialize(deserializedCart);
            
            if(cart != null)
            {
                cart.SerializedCart = serializedCart;
                context.Carts.Entry(cart).State = EntityState.Modified;
            }
            else
            {
                cart = new CartDb
                {
                    Id = new Guid(),
                    UserId = userId,
                    SerializedCart = serializedCart
                };
                context.Carts.Entry(cart).State = EntityState.Added;
            }
            context.SaveChanges();
        }
    }
}
