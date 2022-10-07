using System.Collections.Generic;

namespace EcommerceWebApp.Models
{
    public class ShoppingCart
    {
        public string UserName { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public decimal TotalPrice
        {
            get 
            { 
                decimal totalPrice = 0;
                foreach(var cartItem in Items) totalPrice += cartItem.Price * cartItem.Quantity;
                return totalPrice;
            }
        }

        public ShoppingCart()
        {
        
        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

    }
}
