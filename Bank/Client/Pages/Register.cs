using Bank.Shared;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace Bank.Client.Pages
{
    public partial class Register
    {
        private User User { get; set; } = new User();
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        private async Task HandleValidSubmit()
        {
            // Realize a validação manual do modelo
            if (string.IsNullOrEmpty(User.FirstName) || string.IsNullOrEmpty(User.LastName) || string.IsNullOrEmpty(User.Email) || string.IsNullOrEmpty(User.UPassword))
            {
                Console.WriteLine("Erro: Por favor, preencha todos os campos obrigatórios.");
                return;
            }

            // Chamada do serviço IUserService para adicionar o usuário
            var response = await UserDataService.AddUser(User);

            if (response != null)
            {
                Console.WriteLine("Sucesso: " + JsonSerializer.Serialize(response));
                NavigationManager.NavigateTo($"/");

            }
            else
            {
                Console.WriteLine("Erro: Ocorreu um problema ao salvar o usuário.");
            }
        }
    }
}
