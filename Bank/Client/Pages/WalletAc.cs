using Bank.Client.Services;
using Bank.Shared;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Linq;

namespace Bank.Client.Pages
{
    public partial class WalletAc
    {
        private string CurrentUserIdString;
        private Wallet WalletById;
        private Bank.Shared.Wallet Wallet { get; set; } = new Bank.Shared.Wallet();
        private List<SymbolAc> symbolsWithCurrentUserId = new List<SymbolAc>();

        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private ISymbolAcService SymbolAcService { get; set; }
        private SymbolAc SymbolAc { get; set; } = new SymbolAc();

        protected override async Task OnInitializedAsync()
        {
            CurrentUserIdString = await localStore.GetItemAsync<string>("CurrentUserId");
            if (string.IsNullOrEmpty(CurrentUserIdString))
            {
                NavigationManager.NavigateTo("/Login");
                return;
            }

            if (int.TryParse(CurrentUserIdString, out int currentUserId))
            {
                WalletById = await WalletService.GetWalletId(currentUserId);

                // Carrega os símbolos com o currentUserId
                var allSymbols = await SymbolAcService.GetAllSymbols();
                symbolsWithCurrentUserId = allSymbols.Where(symbol => symbol.WalletId == currentUserId).ToList();
            }
        }



    }
}
