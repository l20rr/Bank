using Bank.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank.Api.Models
{
    public interface IUserModel
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int userId);
        Task<User> AddUser(User user);
        User UpdateUser(User user);
        void DeleteUser(int userId);
    }
}
