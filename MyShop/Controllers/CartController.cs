using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain;
using MyShop.Domain.Entities;
using MyShop.Services;
using System;
using System.Security.Claims;

namespace MyShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext context;
        private readonly IHttpContextAccessor _contextAccessor;
        public CartController(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _contextAccessor = httpContextAccessor;
            this.context = context;
        }

        //Метод для вывода представления корзины
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  //Получаем ID юзера, авторизованно в данный момент
            Cart? cart = CartDbProccesing.GetCartFromDb(context, userId); //Достаем из БД snapshot корзины
            if (cart == null)                                             //Если нет такого snapshot'а, то
            {                                                             //создаем объект корзины и добавляем в сессию, далее
                cart = GetOrCreateCart_Session();                         //добавляем этот объект в базу данных
                CartDbProccesing.SetCartToDb(context, cart, userId);
            }
            else                                                          //Иначе добавляем в сессию объект из БД
                _contextAccessor?.HttpContext?.Session.SetCart(cart);
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(Guid id, string isCart)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);    //Получаем ID юзера, авторизованно в данный момент
            CarInfo? car = context.CarInfo.FirstOrDefault(c => c.Id == id); //Достаем из БД модель машины по id 
            if (car == null)                                                //Если null, то
            {                                                               //выкидываем в представление ошибку
                return NotFound();
            }

            Cart? cart = CartDbProccesing.GetCartFromDb(context, userId);   //Достаем из БД snapshot корзины
            if (cart == null)                                               //Если нет такого snapshot'а, то
                cart = GetOrCreateCart_Session();                           //создаем объект корзины и добавляем в сессию, далее

            CartItem? item = cart?.Items?.FirstOrDefault(c => c.CarId == car.Id);   //Достаем объект товара в корзине из модели корзины
            if (item == null)                                                       //Если null, то 
            {                                                                       //создаем объект с задаными свойствами
                item = new CartItem
                {
                    CarId = car.Id,
                    Name = car.Name,
                    Price = car.Price,
                    Quantity = 1
                };
                cart?.Items?.Add(item);                                     //Добавляем в корзину модель item'а
            }
            else                                                            //Иначе увеличиваем количество товара на 1
                item.Quantity++;

            CartDbProccesing.SetCartToDb(context, cart, userId);            //Добавляем в БД модель корзины 
            _contextAccessor?.HttpContext?.Session.SetCart(cart);           //Также добавляем в сессию

            if (isCart == "true")
                return View("Index", cart);
            else
                return Redirect($"/Home/Show/{car.Id}");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(Guid id, string isCart, string codeWord) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);                //Получаем ID юзера, авторизованно в данный момент

            Cart? cart = CartDbProccesing.GetCartFromDb(context, userId);               //Достаем из БД snapshot корзины
            if (cart == null)                                                           //Если нет такого snapshot'а, то
                cart = _contextAccessor?.HttpContext?.Session.GetCart();                //получаем из сессии объект корзины

            if (cart != null)                                                           //Если объект корзины не найден
            {
                CartItem? item = cart?.Items?.FirstOrDefault(c => c.CarId == id);       //Достаем объект товара в корзине из модели корзины

                if (item == null)                                                       //Если null, то 
                    return NotFound();                                                  //создаем объект с задаными свойствами

                if (codeWord == "dec" && item.Quantity > 1)                             //Если с фронта передается слово "dec" (уменьшение кол-ва товара на одно) и   
                    item.Quantity--;                                                    //количество больше одного
                else
                    cart?.Items?.Remove(item);                                          //Иначе удаляем товар

                CartDbProccesing.SetCartToDb(context, cart, userId);                    //Добавляем в БД модель корзины
                                                                                        
                _contextAccessor?.HttpContext?.Session.SetCart(cart);                   //Также добавляем в сессию
            }

            if (isCart == "true")
                return View("Index", cart);
            else
                return Redirect($"/Home/Show/{id}");
        }

        public IActionResult Checkout()
        {
            var cart = _contextAccessor.HttpContext?.Session.GetCart();
            var products = cart?.Items;
            return View(new Order { CustomerName = User.FindFirstValue(ClaimTypes.Name), Products = products });
        }

        //Метод достающий из сессии корзину или создающий корзину, если нет, и добавляющий в сессию
        private Cart GetOrCreateCart_Session()
        {
            Cart? cart = _contextAccessor?.HttpContext?.Session.GetCart();
            if (cart == null)
            {
                cart = new Cart(new List<CartItem>());
                _contextAccessor?.HttpContext?.Session.SetCart(cart);
            }
            return cart;
        }

    }
}
