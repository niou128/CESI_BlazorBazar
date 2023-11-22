using WebApp.Models;

namespace WebApp.Services
{
    public class CartService
    {
        private List<Product> _cartItems = new List<Product>();

        public IReadOnlyList<Product> CartItems => _cartItems.AsReadOnly();

        public void AddToCart(Product product)
        {
            _cartItems.Add(product);
            product.Stock--;
        }

        public void EmptyCart()
        {
            foreach (var product in _cartItems)
            {
                product.Stock++;
            }
            _cartItems.Clear();
        }
    }
}
