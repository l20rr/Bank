using Bank.Client.Services;
using Bank.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Bank.Client.Pages
{
    public class AuthenticationService
    {
        private readonly ILocalStorageService localStore;
        private readonly NavigationManager navigationManager;
        private readonly UserDataService userDataService;

        public AuthenticationService(ILocalStorageService localStore, NavigationManager navigationManager, UserDataService userDataService)
        {
            this.localStore = localStore;
            this.navigationManager = navigationManager;
            this.userDataService = userDataService;
        }

        public async Task<User> GetAuthenticatedUser()
        {
            string currentUserIdString = await localStore.GetItemAsync<string>("CurrentUserId");

            if (!string.IsNullOrEmpty(currentUserIdString) && int.TryParse(currentUserIdString, out int currentUserId))
            {
                return await userDataService.GetUserId(currentUserId);
            }
            else
            {
                navigationManager.NavigateTo("/Login");
                return null;
            }
        }
    }
}
