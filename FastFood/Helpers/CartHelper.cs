using FastFoodOnline.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace FastFoodOnline.Helpers
{
    public static class CartHelper
    {
        private const string CartSessionKey = "ShoppingCart";

        public static List<CartItem> GetCart(ISession session)
        {
            var cartJson = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }
            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        public static void SaveCart(ISession session, List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            session.SetString(CartSessionKey, cartJson);
        }

        public static void AddToCart(ISession session, CartItem item)
        {
            var cart = GetCart(session);

            // Check if item already exists in cart
            var existingItem = cart.FirstOrDefault(x => x.Id == item.Id && x.Type == item.Type);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            SaveCart(session, cart);
        }

        public static void RemoveFromCart(ISession session, int id, string type)
        {
            var cart = GetCart(session);
            var item = cart.FirstOrDefault(x => x.Id == id && x.Type == type);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(session, cart);
            }
        }

        public static void UpdateQuantity(ISession session, int id, string type, int quantity)
        {
            var cart = GetCart(session);
            var item = cart.FirstOrDefault(x => x.Id == id && x.Type == type);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                SaveCart(session, cart);
            }
        }

        public static void ClearCart(ISession session)
        {
            session.Remove(CartSessionKey);
        }

        public static int GetCartCount(ISession session)
        {
            var cart = GetCart(session);
            return cart.Sum(x => x.Quantity);
        }

        public static decimal GetCartTotal(ISession session)
        {
            var cart = GetCart(session);
            return cart.Sum(x => x.TotalPrice);
        }
    }
}
