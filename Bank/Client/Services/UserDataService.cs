using Bank.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace Bank.Client.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly HttpClient _httpClient;

        public UserDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> GetLongUserList()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<User>>($"api/users/long");
        }

        public async Task<User> AddUser(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", user);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }

            return null;
        }
        public async Task UpdateUser(User user)
        {
            var userJson =
                new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync("api/users", userJson);
        }

        public async Task DeleteUser(int userId)
        {
            await _httpClient.DeleteAsync($"api/users/{userId}");
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<User>>
                   (await _httpClient.GetStreamAsync($"api/users"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public Task<IEnumerable<User>> IUserDataService()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserId(int userId)
        {
            return await JsonSerializer.DeserializeAsync<User>
               (await _httpClient.GetStreamAsync($"api/users/{userId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value)
        {
            return _httpClient.PostAsJsonAsync(requestUri, value);
        }



    }
}
