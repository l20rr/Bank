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
                    
                    return BadRequest("Password and Confirm Password are required.");
                }

                if (user.UPassword != user.ConfirmPassword)
                {
                   
                    return BadRequest("Password and Confirm Password do not match.");
                }

                // Gera o hash da senha usando bcrypt
                string hashedPassword = HashPassword(user.UPassword);
                user.UPassword = hashedPassword;

                // Gera o hash da confirmação de senha usando bcrypt (mesmo salt usado para a senha principal)
                string hashedConfirmPassword = HashPassword(user.ConfirmPassword);
                user.ConfirmPassword = hashedConfirmPassword; 

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
            
            int workFactor = 10; 

            // Gera o salt 
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

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;

            if (!string.IsNullOrEmpty(user.UPassword) && !string.IsNullOrEmpty(user.ConfirmPassword))
            {
                if (user.UPassword != user.ConfirmPassword)
                {
                    return BadRequest("Password and Confirm Password do not match.");
                }

                // Gera o hash da nova senha usando bcrypt
                string hashedPassword = HashPassword(user.UPassword);
                userToUpdate.UPassword = hashedPassword; 
            }

            _userModel.UpdateUser(userToUpdate);

            return NoContent(); 
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

            return NoContent();
        }
    }
}
