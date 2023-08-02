using Bank.Shared;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Bank.Client.Pages
{
    public partial class Register
    {
        private User User { get; set; } = new User();
        private string CurrentUserIdString;
        private Bank.Shared.Wallet Wallet { get; set; } = new Bank.Shared.Wallet();
        private Wallet walletToFind;
        [Inject]
        private NavigationManager NavigationManager { get; set; }


        private string firstNameValidationMessage = string.Empty;
        private string lastNameValidationMessage = string.Empty;
        private string emailValidationMessage = string.Empty;
        private string passwordValidationMessage = string.Empty;
        private string confirmPasswordValidationMessage = string.Empty;
        private string walletNameValidationMessage = string.Empty;
        private bool showError = false;
        private async Task HandleValidSubmit()
        {
            bool isFirstNameValid = ValidateFirstName();
            bool isLastNameValid = ValidateLastName();
            bool isEmailValid = ValidateEmail();
            bool isPasswordValid = ValidatePassword();
            bool isConfirmPasswordValid = ValidateConfirmPassword();
            bool isWalletNameValid = ValidateWalletName();

            if (isFirstNameValid && isLastNameValid && isEmailValid && isPasswordValid && isConfirmPasswordValid && isWalletNameValid)
            {
                
                showError = false;
              var response = await UserDataService.AddUser(User);
                await localStorage.SetItemAsync("CurrentUserId", response.UserId);
                await Criarcarteira(response.UserId);
                Console.WriteLine("Sucesso: " + JsonSerializer.Serialize(response));
                NavigationManager.NavigateTo("/", forceLoad: true);
            }
            else
            {
                // error message
                showError = true;
            }

        }
        private string GetRegStyle()
        {
            
            if (showError == true)
            {
                return "error-mode"; 
            }
            else
            {
                return "base-style"; 
            }
        }


        private bool ValidateFirstName()
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

        private bool ValidateLastName()
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

        private bool ValidateWalletName()
        {
            
            if (string.IsNullOrEmpty(Wallet.WalletName))
            {
                walletNameValidationMessage = "O nome da carteira não pode ser vazio.";
                return false;
            }
            else
            {
                walletNameValidationMessage = string.Empty;
                return true;
            }
        }
        private async Task Criarcarteira(int userId)
        {
 
            if (!string.IsNullOrWhiteSpace(Wallet.WalletName))
            {
                
                Wallet.UserId = userId;

         
                var response = await WalletService.AddWallet(Wallet);

                if (response != null)
                {
                    Console.WriteLine("Sucesso: " + JsonSerializer.Serialize(response));
                 }
                else
                {
                    Console.WriteLine("Erro: Ocorreu um problema ao criar a carteira.");
                }
            }
            else
            {
                Console.WriteLine("Erro: O nome da carteira não pode ser vazio.");
            }
        }


    }
}
