using Bank.Client.Services;
using Bank.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bank.Client.Pages
{
    public partial class Details
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Symbol { get; set; }

        private async Task Search()
        {
            NavigationManager.NavigateTo($"/Details/{Symbol}");
            await FetchData();
        }


        private List<Data> datas;
        protected override async Task OnInitializedAsync()
        {
            await FetchData();
        }

        protected override async Task OnParametersSetAsync()
        {
            await FetchData();
        }
        private async Task FetchData()
        {
            string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY_ADJUSTED&symbol={Symbol}&apikey=SGIYAYJ5YOITH6Q6";
            Uri queryUri = new Uri(QUERY_URL);

            var response = await HttpClient.GetStringAsync(queryUri);
            var jsonDocument = JsonDocument.Parse(response);

            datas = new List<Data>();

            foreach (var timeSeries in jsonDocument.RootElement.GetProperty("Monthly Adjusted Time Series").EnumerateObject())
            {
                var data = new Data
                {
                    MetaData = new MetaData
                    {
                        Symbol = jsonDocument.RootElement.GetProperty("Meta Data").GetProperty("2. Symbol").GetString(),
                        LastRefreshed = jsonDocument.RootElement.GetProperty("Meta Data").GetProperty("3. Last Refreshed").GetString()
                    },
                    TimeSeries = new Dictionary<string, TimeSeriesItem>()
                };

                var time = timeSeries.Name;
                var values = timeSeries.Value;

                var timeSeriesItem = new TimeSeriesItem
                {
                    Open = values.GetProperty("1. open").GetString(),
                    High = values.GetProperty("2. high").GetString(),
                    Low = values.GetProperty("3. low").GetString(),
                    Close = values.GetProperty("4. close").GetString()
                };

                data.TimeSeries.Add(time, timeSeriesItem);
                datas.Add(data);
            }

            NavigationManager.NavigateTo($"/Details/{Symbol}");
        }

        public class Data
        {
            public MetaData MetaData { get; set; }
            public Dictionary<string, TimeSeriesItem> TimeSeries { get; set; }
        }

        public class MetaData
        {
            public string Symbol { get; set; }
            public string LastRefreshed { get; set; }
        }

        public class TimeSeriesItem
        {
            public string Open { get; set; }
            public string High { get; set; }
            public string Low { get; set; }
            public string Close { get; set; }
        }
        private Wallet Wallet{ get; set; } = new Wallet();

        [Inject]
        private IWalletService WalletService { get; set; }

        [Inject]
        private Blazored.LocalStorage.ILocalStorageService localStore { get; set; }

        private async Task CheckWallets(string symbolName)
        {
            var CurrentUserIdString = await localStore.GetItemAsync<string>("CurrentUserId");
            if (int.TryParse(CurrentUserIdString, out int currentUserId))
            {
                var wallets = await WalletService.GetAllWallets();
                var walletToMatch = wallets.FirstOrDefault(wallet => wallet.UserId == currentUserId);

                if (walletToMatch != null)
                {
                    await AddSymbol(walletToMatch.WalletId, symbolName);
                }
                else
                {
                    Console.WriteLine("Não existe uma carteira correspondente ao usuário.");
                }
            }
            else
            {
                Console.WriteLine("O valor de CurrentUserIdString não é válido.");
            }
        }
        private async Task AddSymbol(int walletId, string symbolName)
        {
            // Aqui você pode chamar o serviço para adicionar o símbolo à carteira usando o walletId e o symbolName
            // Exemplo:
            // var response = await WalletService.AddSymbolToWallet(walletId, symbolName);
            // Verifique o serviço de carteira para saber como adicionar o símbolo à carteira.
            Console.WriteLine($"Adicionando o símbolo {symbolName} à carteira com WalletId: {walletId}");
        }



    }
}