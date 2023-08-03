using Bank.Shared;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Bank.Client.Services
{
    public class WalletService : IWalletService
    {
        private readonly HttpClient _httpClient;
        public WalletService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Wallet> AddWallet(Wallet wallet)
        {
            var response = await _httpClient.PostAsJsonAsync("api/wallets", wallet);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Wallet>();
            }

            return null;
        }

        public async Task DeleteWallet(int walletId)
        {
            await _httpClient.DeleteAsync($"api/wallets/{walletId}");
        }

        public async Task<IEnumerable<Wallet>> GetAllWallets()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Wallet>>
                   (await _httpClient.GetStreamAsync($"api/wallet"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        }

        public async Task<Wallet> GetWalletId(int walletId)
        {
            return await JsonSerializer.DeserializeAsync<Wallet>
              (await _httpClient.GetStreamAsync($"api/wallets/{walletId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public Task<IEnumerable<Wallet>> IWalletService()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateWallet(Wallet wallet)
        {

            var userJson = new StringContent(JsonSerializer.Serialize(wallet), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync("api/wallets", userJson);

        }
    }
}
