using MyShop.Domain.Entities;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace MyShop.Services
{
    public static class SessionExtensions
    {
        private const string CartSessionKey = "Cart";

        public static Cart? GetCart(this ISession session)
        {
            byte[]? cartData = session.Get(CartSessionKey);
            return cartData == null ? null : JsonConvert.DeserializeObject<Cart>(Encoding.UTF8.GetString(cartData));
        }

        public static void SetCart(this ISession session, Cart? cart)
        {
            session.Set(CartSessionKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cart)));
        }
    }
}
