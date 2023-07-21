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
        private User User { get; set; } = new User();

        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string UserId { get; set; }

        private async Task Edit()
        {
            try
            {
                // Conversão do UserId para o tipo apropriado (exemplo: int)
                int userId = int.Parse(UserId);

                // Chamada assíncrona do serviço IUserService para adicionar o usuário
                var response =  UserDataService.UpdateUser(User);

                if (response != null)
                {
                    Console.WriteLine("Sucesso: " + JsonSerializer.Serialize(response));

                    // Navegação para a página de perfil do usuário editado
                    NavigationManager.NavigateTo($"/profile/{userId}");
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

        protected async Task Del()
        {
            await UserDataService.DeleteUser(User.UserId);

        }
    }
}
