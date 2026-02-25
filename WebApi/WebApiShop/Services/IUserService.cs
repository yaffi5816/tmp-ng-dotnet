using DTO;

namespace Services
{
    public interface IUserService
    {
        Task<UserReadOnlyDTO> AddUser(UserRegisterDTO user);
        Task DeleteUser(int currentUserId, int userId);
        Task<IEnumerable<UserReadOnlyDTO>> GetAsync(int currentUserId);
        Task<UserReadOnlyDTO> GetUserById(int id);
        Task<UserReadOnlyDTO> Login(UserLoginDTO user);
        Task UpdateUser(int id, UserUpdateDTO user);
    }
}