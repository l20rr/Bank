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
                // Todos os campos são válidos, continua com o salvamento do usuário e da carteira
                showError = false;
                // Implementar a lógica para salvar o usuário e a carteira aqui
            }
            else
            {
                // Se algum campo não for válido, exibe mensagem de erro
                showError = true;
            }

            // Chamada do serviço IUserService para adicionar o usuário
            var response = await UserDataService.AddUser(User);

            if (response != null)
            {
                await localStorage.SetItemAsync("currentUserId", response.UserId);
                await Criarcarteira(response.UserId);
                Console.WriteLine("Sucesso: " + JsonSerializer.Serialize(response));
                NavigationManager.NavigateTo("/", forceLoad: true);

            }
            else
            {
                Console.WriteLine("Erro: Ocorreu um problema ao salvar o usuário.");
            }
        
        }


        private bool ValidateFirstName()
        {
            // Implemente a lógica para validar o primeiro nome (se necessário)
            // Retorna true se for válido, caso contrário, atualiza a mensagem de erro e retorna false
            // Exemplo:
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
            // Implemente a lógica para validar o último nome (se necessário)
            // Retorna true se for válido, caso contrário, atualiza a mensagem de erro e retorna false
            // Exemplo:
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
            // Expressão regular para validar o formato do email
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
            // Implemente a lógica para validar a senha (se necessário)
            // Retorna true se for válida, caso contrário, atualiza a mensagem de erro e retorna false
            // Exemplo:
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
            // Implemente a lógica para validar a confirmação da senha (se necessário)
            // Retorna true se for válida, caso contrário, atualiza a mensagem de erro e retorna false
            // Exemplo:
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
            // Implemente a lógica para validar o nome da carteira (se necessário)
            // Retorna true se for válido, caso contrário, atualiza a mensagem de erro e retorna false
            // Exemplo:
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
                // Preencha o UserId da carteira com o valor obtido do parâmetro (userId)
                Wallet.UserId = userId;

                // Chamar o serviço para adicionar a carteira
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
