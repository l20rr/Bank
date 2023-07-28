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


        /*Tests Unitarios*/
        public bool ValidatePasswords(string password, string confirmPassword)
        {
            return password == confirmPassword;
        }

        public bool ValidateUsers(int userId, string firstName, string lastName, string email, string password, string confirmPassword, DateTime joinedDate)
        {
            // Aqui você pode definir suas regras de validação
            // Vou fornecer um exemplo simples, mas você pode adicionar mais lógica conforme suas necessidades.

            if (string.IsNullOrWhiteSpace(firstName))
                return false; // O primeiro nome é obrigatório.

            if (string.IsNullOrWhiteSpace(lastName))
                return false; // O sobrenome é obrigatório.

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                return false; // O email é obrigatório e deve estar em um formato válido.

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return false; // A senha é obrigatória e deve ter pelo menos 6 caracteres.

            if (password != confirmPassword)
                return false; // A senha e a confirmação de senha devem ser iguais.

            if (joinedDate > DateTime.Now)
                return false; // A data de entrada não pode ser no futuro.

            // Se todas as validações passaram, o usuário é considerado válido.
            return true;
        }
        private bool IsValidEmail(string email)
        {
            // Aqui você pode adicionar lógica para verificar se o email tem um formato válido.
            // Neste exemplo, vamos fazer uma validação simples verificando se contém '@' e '.'.
            return email.Contains("@") && email.Contains(".");
        }

        public Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value)
        {
            return _httpClient.PostAsJsonAsync(requestUri, value);
        }



    }
}
