﻿using Bank.Client.Services;
using Bank.Shared;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Linq;

namespace Bank.Client.Pages
{
    public partial class WalletAc
    {
        private string CurrentUserIdString;
        public Wallet WalletById;
        public string Symbol;
        private Bank.Shared.Wallet Wallet { get; set; } = new Bank.Shared.Wallet();
        public List<SymbolAc> symbolsWithCurrentUserId = new List<SymbolAc>();

        public bool EditAcs = false;
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        public ISymbolAcService SymbolAcService { get; set; }
        public SymbolAc SymbolAc { get; set; } = new SymbolAc();
        private User User { get; set; } = new User();
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


        private async Task deleteSymbol(string SymbolName)
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

                
                var allSymbols = await SymbolAcService.GetAllSymbols();

                // Fetch currentUserId e SymbolName
                var symbolsNameId = allSymbols.Where(symbol => symbol.WalletId == currentUserId && symbol.SymbolName == SymbolName).ToList();

                if (symbolsNameId.Any())
                {
                    //Search for wallet symbols
                    foreach (var symbol in symbolsNameId)
                    {
                        
                        SymbolAcService.DeleteSymbol(symbol.SymbolId);
                    }
                    NavigationManager.NavigateTo("/wallet", forceLoad: true);
                }
             
            }
        
        }

        private void EditAc()
        {
            EditAcs = !EditAcs;

        }

    }
}
