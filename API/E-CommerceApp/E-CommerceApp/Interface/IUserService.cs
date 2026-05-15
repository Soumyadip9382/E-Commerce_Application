using E_CommerceApp.Domain;
using static E_CommerceApp.Domain.DTOs.UserDTO;

namespace E_CommerceApp.Interface
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<int> CreateUser(CreateUser user);
        Task<User?> GetUserById(int id);
        Task<User?> UpdateUser(int id, UpdateUser user);
        Task<bool> DeleteUser(int id);
        Task<User> Login(Loggedin user);
        Task<string> UpdatePassword(int id, UpdatePassword model);
    }
}