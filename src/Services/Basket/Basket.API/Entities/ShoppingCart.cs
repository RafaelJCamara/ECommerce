using System.Collections.Generic;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new();

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                foreach (var cartItem in Items) totalPrice += cartItem.Price * cartItem.Quantity;
                return totalPrice;
            }
        }
    }
}