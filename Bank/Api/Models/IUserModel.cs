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
        Task<User> UpdateUser(User user);
        Task<User>DeleteUser(int userId);
    }
}
