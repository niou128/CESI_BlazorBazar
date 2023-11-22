using System.Text.Json;
using WebApp.Models;

namespace WebApp.Services
{
    public class ProductServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string ProductApiUrl;

        public ProductServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            ProductApiUrl = "https://localhost:32786/Products/";
        }

        public async Task<Product[]> GetProducts()
        {
            var response = await _httpClient.GetAsync($"{ProductApiUrl}");
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Product[]>(content, options);
        }

        public async Task<Product> GetProduct(int id)
        {
            var response = await _httpClient.GetAsync($"{ProductApiUrl}{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(content);
        }
    }
}
