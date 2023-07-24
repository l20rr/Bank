using Bank.Client.Services;
using Bank.Shared;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace Bank.Client.Pages
{
    public partial class WalletAc
    {
        private string CurrentUserIdString;
        private Bank.Shared.Wallet Wallet { get; set; } = new Bank.Shared.Wallet();
        private Wallet walletToFind;
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentUserIdString = await localStore.GetItemAsync<string>("CurrentUserId");
            if (string.IsNullOrEmpty(CurrentUserIdString))
            {
                NavigationManager.NavigateTo("/Login");
            }

            if (int.TryParse(CurrentUserIdString, out int currentUserId))
            {
                var allWallets = await WalletService.GetAllWallets();

                // Find the wallet with LINQ
                var userIdToFind = currentUserId; // Você pode substituir o valor aqui pelo valor desejado do UserId
                walletToFind = allWallets.FirstOrDefault(wallet => wallet.UserId == userIdToFind);
            }
        }

        private async Task Criarcarteira()
        {
            if (!string.IsNullOrWhiteSpace(Wallet.WalletName))
            {
                // Preencha o UserId da carteira com o valor obtido do localStore (CurrentUserIdString)
                if (int.TryParse(CurrentUserIdString, out int userId))
                {
                    Wallet.UserId = userId;

                    // Chamar o serviço para adicionar a carteira
                    var response = await WalletService.AddWallet(Wallet);

                    if (response != null)
                    {
                        Console.WriteLine("Sucesso: " + JsonSerializer.Serialize(response));
                        // Você pode adicionar aqui uma mensagem de sucesso ou redirecionar para outra página
                        // Após adicionar a carteira, você pode atualizar a lista de carteiras exibida na página, se houver uma lista.
                    }
                    else
                    {
                        Console.WriteLine("Erro: Ocorreu um problema ao criar a carteira.");
                    }
                }
                else
                {
                    Console.WriteLine("Erro: O valor de CurrentUserIdString não é válido.");
                }
            }
            else
            {
                Console.WriteLine("Erro: O nome da carteira não pode ser vazio.");
            }
        }
    }
}
