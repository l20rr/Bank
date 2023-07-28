using Bank.Client.Services;
using Bank.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        private List<Data> datas;
        private IEnumerable<KeyValuePair<string, TimeSeriesItem>> filteredData = Enumerable.Empty<KeyValuePair<string, TimeSeriesItem>>();
        private bool showFilteredData = false;
        private async Task Search()
        {
            NavigationManager.NavigateTo($"/Details/{Symbol}");
            await FetchData();
        }

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
            //string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY_ADJUSTED&symbol={Symbol}&apikey=SGIYAYJ5YOITH6Q6";
            //string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY_ADJUSTED&symbol={Symbol}&apikey=NZWQHLKWKIOXGT5K";
            string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY_ADJUSTED&symbol=IBM&apikey=demo";
            Uri queryUri = new Uri(QUERY_URL);

            var response = await HttpClient.GetStringAsync(queryUri);
            var jsonDocument = JsonDocument.Parse(response);

            datas = new List<Data>();

            dados = new Dictionary<string, TimeSeriesItem>();

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
            filteredData = dados;
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

        private string CurrentUserIdString { get; set; }
        private SymbolAc SymbolAc { get; set; } = new SymbolAc();

        [Inject]
        private IWalletService WalletService { get; set; }

        [Inject]
        private ISymbolAcService SymbolAcService { get; set; }

        [Inject]
        private Blazored.LocalStorage.ILocalStorageService localStore { get; set; }

        private async Task GetUserId()
        {
            CurrentUserIdString = await localStore.GetItemAsync<string>("CurrentUserId");
        }

        private bool saveAc ;
        private IEnumerable<KeyValuePair<string, TimeSeriesItem>> dados;

        public async Task AddSymbolToWallet()
        {
            // Fetch  CurrentUserId  localStore
            await GetUserId();

            if (!string.IsNullOrEmpty(CurrentUserIdString))
            {
                if (int.TryParse(CurrentUserIdString, out int walletId))
                {
                    // Use the Symbol property directly as symbolName
                    string symbolName = Symbol;

                    // Call AddSymbol with walletId and symbolName
                    await AddSymbolAc(walletId, symbolName);
                    
                }
                else
                {
                    Console.WriteLine("CurrentUserId is not a valid integer.");
                }
            }
            else
            {
                NavigationManager.NavigateTo("/Login");
            }
        }

        public async Task AddSymbolAc(int walletId, string symbolName)
        {
            SymbolAc.WalletId = walletId;
            SymbolAc.SymbolName = symbolName;
            var symbols = await SymbolAcService.GetAllSymbols();
            var symbolExists = symbols.Any(s => s.WalletId == walletId && s.SymbolName == symbolName);

            if (!symbolExists)
            {
                var response = await SymbolAcService.AddSymbol(SymbolAc);
                if (response != null)
                {
                    saveAc = true;
                }
            }
            else
            {
                saveAc = false;
            }
        }




        private void FilterAllData()
        {
            filteredData = datas.SelectMany(data => data.TimeSeries);
            showFilteredData = true;
        }

        private void FilterBy(Func<TimeSeriesItem, decimal> selector)
        {
            var maxValue = datas.Max(data => data.TimeSeries.Max(item =>
            {
                decimal value;
                return decimal.TryParse(selector(item.Value).ToString(), out value) ? value : decimal.MinValue;
            }));

            filteredData = datas.SelectMany(data => data.TimeSeries)
                                .Where(item =>
                                {
                                    decimal value;
                                    return decimal.TryParse(selector(item.Value).ToString(), out value) && value == maxValue;
                                });

            showFilteredData = true;
        }



        private void FilterByMaiorClose()
        {
            filteredData = datas.SelectMany(data => data.TimeSeries)
                                .OrderByDescending(item => decimal.Parse(item.Value.Close, CultureInfo.InvariantCulture))
                                .Take(1);
            showFilteredData = true;
        }

        private void FilterByMaiorOpen()
        {
            filteredData = datas.SelectMany(data => data.TimeSeries)
                                .OrderByDescending(item => decimal.Parse(item.Value.Open, CultureInfo.InvariantCulture))
                                .Take(1);
            showFilteredData = true;
        }


        private void FilterByMaisAntiga()
        {
            filteredData = datas.SelectMany(data => data.TimeSeries)
                                .OrderBy(item => DateTime.Parse(item.Key))
                                .Take(1);
            showFilteredData = true;
        }


        private void FilterByMaisBaixa()
        {
            filteredData = datas.SelectMany(data => data.TimeSeries)
                                .OrderBy(item => decimal.Parse(item.Value.Low, CultureInfo.InvariantCulture))
                                .Take(1);
            showFilteredData = true;
        }

        private void OriginData()
        {
            showFilteredData = false;
        }
    }
}