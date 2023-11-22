using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using WebApp.Models;

namespace WebApp.Services
{
    public class UserServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string UserServiceBaseUrl;

        public UserServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            UserServiceBaseUrl = "https://localhost:32784/User/";
        }

        public async Task<User> Register(User user)
        {
            var userJson = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{UserServiceBaseUrl}register", userJson);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseBody);
        }

        public async Task<User> GetUser(int id)
        {
            var response = await _httpClient.GetAsync($"{UserServiceBaseUrl}{id}");

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseBody);
        }

        public async Task UpdateUser(int id, User user)
        {
            var userJson = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{UserServiceBaseUrl}{id}", userJson);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUser(int id)
        {
            var response = await _httpClient.DeleteAsync($"{UserServiceBaseUrl}{id}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<string> Login(string username, string password)
        {
            var loginRequest = new User
            {
                Username = username,
                Password = password,
                Email = string.Empty
            };

            var loginJson = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{UserServiceBaseUrl}login", loginJson);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public int? GetUserIdFromToken(string token)
        {
            var handler = new JsonWebTokenHandler();
            var jsonToken = handler.ReadJsonWebToken(token);

            var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;


            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}
