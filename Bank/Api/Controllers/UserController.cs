using Bank.Api.Models;
using Bank.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Bank.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserModel _userModel;

        public UserController(IUserModel userModel)
        {
            _userModel = userModel;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_userModel.GetAllUsers());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching users: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            try
            {
                return Ok(_userModel.GetUserById(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the user: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.UPassword) || string.IsNullOrEmpty(user.ConfirmPassword))
                {
                    // Verifique se a senha e a confirmação de senha foram fornecidas
                    return BadRequest("Password and Confirm Password are required.");
                }

                if (user.UPassword != user.ConfirmPassword)
                {
                    // Verifique se a senha e a confirmação de senha correspondem
                    return BadRequest("Password and Confirm Password do not match.");
                }

                // Gera o hash da senha usando bcrypt
                string hashedPassword = HashPassword(user.UPassword);
                user.UPassword = hashedPassword; // Atualiza a senha com o valor hash antes de adicioná-la ao banco de dados

                // Gera o hash da confirmação de senha usando bcrypt (mesmo salt usado para a senha principal)
                string hashedConfirmPassword = HashPassword(user.ConfirmPassword);
                user.ConfirmPassword = hashedConfirmPassword; // Atualiza a confirmação de senha com o valor hash

                var newUser = await _userModel.AddUser(user);
                return CreatedAtAction(nameof(GetUserById), new { userId = newUser.UserId }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the user: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            // Define o trabalho (complexidade) do bcrypt
            int workFactor = 10; // Pode ser ajustado conforme necessário

            // Gera o salt aleatório
            string salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor);

            // Gera o hash da senha com o salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            if (user.FirstName == string.Empty || user.LastName == string.Empty)
            {
                ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userToUpdate = _userModel.GetUserById(user.UserId);

            if (userToUpdate == null)
                return NotFound();

            _userModel.UpdateUser(user);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            if (id == 0)
                return BadRequest();

            var employeeToDelete = _userModel.GetUserById(id);
            if (employeeToDelete == null)
                return NotFound();

            _userModel.DeleteUser(id);

            return NoContent();//success
        }
    }
}
