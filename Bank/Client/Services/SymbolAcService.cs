using Bank.Shared;
using System.Net.Http.Json;
using System.Text.Json;

namespace Bank.Client.Services
{
    public class SymbolAcService : ISymbolAcService
    {
        private readonly HttpClient _httpClient;
        public SymbolAcService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<SymbolAc> AddSymbol(SymbolAc symbolAc)
        {
            var response = await _httpClient.PostAsJsonAsync("api/symbols", symbolAc);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SymbolAc>();
            }

            return null;
        }

        public async Task DeleteSymbol(int symbolId)
        {
            await _httpClient.DeleteAsync($"api/wallets/{symbolId}");
        }

        public async Task<IEnumerable<SymbolAc>> GetAllSymbols()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<SymbolAc>>
                 (await _httpClient.GetStreamAsync($"api/symbols"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


        }

        public Task<IEnumerable<SymbolAc>> ISymbolAcService()
        {
            throw new NotImplementedException();
        }
    }
}
