using Bank.Client.Services;
using Bank.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bank.Client.Pages
{
    public partial class EditUser
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private User User { get; set; } = new User();

        [Inject]
        public IUserDataService UserDataService { get; set; }

        private string CurrentUserIdString;

        protected async override Task OnInitializedAsync()
        {
            CurrentUserIdString = await localStore.GetItemAsync<string>("CurrentUserId");
            if (!string.IsNullOrEmpty(CurrentUserIdString) && int.TryParse(CurrentUserIdString, out int CurrentUserId))
            {
                User = await UserDataService.GetUserId(CurrentUserId);
            }
            else
            {
                NavigationManager.NavigateTo("/Login");
            }
        }

        private async Task Edit()
        {
            try
            {
                // Chamada assíncrona do serviço IUserService para atualizar o usuário
                var response =  UserDataService.UpdateUser(User);

                if (response != null)
                {
                    Console.WriteLine("Sucesso: " + JsonSerializer.Serialize(response));

                    // Navegação para a página de perfil do usuário editado
                    NavigationManager.NavigateTo("/profile", forceLoad: true);
                }
                else
                {
                    Console.WriteLine("Erro: Ocorreu um problema ao salvar o usuário.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        private async Task Del()
        {
          
            await UserDataService.DeleteUser(User.UserId);
            await localStore.RemoveItemAsync("CurrentUserId");
            NavigationManager.NavigateTo("/", forceLoad: true);
        }
    }
}
