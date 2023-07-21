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
 
        private int CurrentUserId { get; set; } 
        private User User { get; set; } = new User();
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        private bool showError = false;
     
     
        private async Task LoginU()
        {
            if (string.IsNullOrEmpty(User.Email) || string.IsNullOrEmpty(User.UPassword))
            {
                showError = true;
               
            }

            // Fetch  database
            var allUsers = await UserDataService.GetAllUsers();

            // Find the user with LINQ
            var userToLogin = allUsers.FirstOrDefault(u => u.Email == User.Email && u.UPassword == User.UPassword);

            if (userToLogin != null)
            {
                Console.WriteLine("entrou");
                CurrentUserId = userToLogin.UserId;
                Console.WriteLine("CurrentUserId: " + CurrentUserId);

                NavigationManager.NavigateTo("/");
            }
            else
            {
                showError = true;
            }
        }
    }
}
