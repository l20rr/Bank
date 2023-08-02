using Bank.Client.Services;
using Bank.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bank.Client.Pages
{
    public partial class EditUser
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private User User { get; set; } = new User();

        private Wallet Wallet { get; set; } 

        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public IWalletService WalletService { get; set; }

        private string CurrentUserIdString;


        private string firstNameValidationMessage = string.Empty;
        private string lastNameValidationMessage = string.Empty;
        private string emailValidationMessage = string.Empty;
        private string passwordValidationMessage = string.Empty;
        private string confirmPasswordValidationMessage = string.Empty;

        private bool showError = false;

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
            bool isFirstNameValid = ValidateFirstName();
            bool isLastNameValid = ValidateLastName();
            bool isEmailValid = ValidateEmail();
            bool isPasswordValid = ValidatePassword();
            bool isConfirmPasswordValid = ValidateConfirmPassword();



            if (isFirstNameValid && isLastNameValid && isEmailValid && isPasswordValid && isConfirmPasswordValid)
            {

                showError = false;

            }
            else
            {

                showError = true;
            }
            try
            {

                var response = UserDataService.UpdateUser(User);

                if (response != null)
                {
                    Console.WriteLine("Sucesso: " + JsonSerializer.Serialize(response));


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

        public bool ValidateFirstName()
        {

            if (string.IsNullOrEmpty(User.FirstName) || !char.IsUpper(User.FirstName[0]))
            {
                firstNameValidationMessage = "Insira o primeiro nome corretamente (deve iniciar com letra maiúscula).";
                return false;
            }
            else
            {
                firstNameValidationMessage = string.Empty;
                return true;
            }
        }

        public bool ValidateLastName()
        {

            if (string.IsNullOrEmpty(User.LastName) || !char.IsUpper(User.LastName[0]))
            {
                lastNameValidationMessage = "Insira o último nome corretamente (deve iniciar com letra maiúscula).";
                return false;
            }
            else
            {
                lastNameValidationMessage = string.Empty;
                return true;
            }
        }

        private bool ValidateEmail()
        {
            // regex email
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            bool isEmailValid = Regex.IsMatch(User.Email, emailPattern);

            if (!isEmailValid)
            {
                emailValidationMessage = "Digite um email válido.";
            }
            else
            {
                emailValidationMessage = string.Empty;
            }

            return isEmailValid;
        }

        private bool ValidatePassword()
        {

            string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$";
            bool isPasswordValid = Regex.IsMatch(User.UPassword, passwordPattern);

            if (!isPasswordValid)
            {
                passwordValidationMessage = "A senha precisa de 6 caracteres, pelo menos 1 letra, 1 número e 2 caracteres especiais.";
                return false;
            }
            else
            {
                passwordValidationMessage = string.Empty;
                return true;
            }
        }

        private bool ValidateConfirmPassword()
        {

            if (User.UPassword != User.ConfirmPassword)
            {
                confirmPasswordValidationMessage = "As senhas não coincidem.";
                return false;
            }
            else
            {
                confirmPasswordValidationMessage = string.Empty;
                return true;
            }
        }

        protected async Task Del()
        {
            await UserDataService.DeleteUser(User.UserId);
            //await WalletService.DeleteWallet(Wallet.WalletId);
            await localStore.RemoveItemAsync("CurrentUserId");
            NavigationManager.NavigateTo("/", forceLoad: true);
            
        }
    }
}
