using Bank.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Api.Models
{
    public class UserModel : IUserModel
    {
        private readonly AppDbContext _appDbContext;

        public UserModel(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _appDbContext.Users;
        }

     

        public async Task<User> AddUser(User user)
        {
           var result = await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public User UpdateUser(User user)
        {
            var foundUser = _appDbContext.Users.FirstOrDefault(e => e.UserId == user.UserId);

            if (foundUser != null)
            {

                foundUser.Email = user.Email;
                foundUser.FirstName = user.FirstName;
                foundUser.LastName = user.LastName;
                foundUser.UPassword = user.UPassword;
                foundUser.UserId = user.UserId; 
                foundUser.ConfirmPassword = user.ConfirmPassword;   
                foundUser.JoinedDate = user.JoinedDate; 


                _appDbContext.SaveChangesAsync();

                return foundUser;
            }

            return null;
        }

        public void DeleteUser(int userId)
        {
            var foundEmployee = _appDbContext.Users.FirstOrDefault(e => e.UserId == userId);
            if (foundEmployee == null) return;

            _appDbContext.Users.Remove(foundEmployee);
            _appDbContext.SaveChanges();
        }

        public User GetUserById(int userId)
        {
            return _appDbContext.Users.FirstOrDefault(c => c.UserId == userId);
        }
    }
}
