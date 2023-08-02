using Bank.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Bank.Client.Pages
{

    public partial class Login
    {
 
        public int CurrentUserId { get; set; } 
        private User User { get; set; } = new User();
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        private bool showError = false;


        private async Task LoginU()
        {
            if (string.IsNullOrEmpty(User.Email) || string.IsNullOrEmpty(User.UPassword))
            {
                showError = true;
                return;
            }

            // Fetch database
            var allUsers = await UserDataService.GetAllUsers();

          
            // Find the user with linq
            var userToLogin = allUsers.FirstOrDefault(u => u.Email == User.Email);

            if (userToLogin != null)
            {
                // Verify the password using bcrypt
                bool isPasswordValid = VerifyPassword(User.UPassword, userToLogin.UPassword);

                if (isPasswordValid)
                {
                    Console.WriteLine("entrou");
                    CurrentUserId = userToLogin.UserId;

                    await localStore.SetItemAsync("CurrentUserId", CurrentUserId.ToString());

                    NavigationManager.NavigateTo("/", forceLoad: true);
                }
                else
                {
                    showError = true;
                }
            }
            else
            {
                showError = true;
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            // Verifica se a senha corresponde ao hash
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
