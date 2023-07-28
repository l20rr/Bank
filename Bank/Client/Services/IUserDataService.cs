using Bank.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Client.Services
{
    public interface IUserDataService
    {
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value);
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUserId(int userId);

        Task<IEnumerable<User>> GetLongUserList();
        Task<User> AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int userId);
       
        Task<IEnumerable<User>> IUserDataService();
      
    }
}
