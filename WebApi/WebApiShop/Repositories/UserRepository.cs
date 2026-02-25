using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        string filePath = "..\\WebApiShop\\FileUsers.txt";

        DashGen2026Context _dashGen2026Context;

        public UserRepository(DashGen2026Context dashGen2026Context)
        {
            _dashGen2026Context = dashGen2026Context;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _dashGen2026Context.Users.FindAsync(id);
            return user;
        }
        public async Task<User> AddUser(User user)
        {
            await _dashGen2026Context.Users.AddAsync(user);
            await _dashGen2026Context.SaveChangesAsync();
            return user;
        }
        public async Task<User> Login(User loginUser)
        {
            var user = await _dashGen2026Context.Users.FirstOrDefaultAsync(x=>x.UserName == loginUser.UserName &&
            x.Password== loginUser.Password);
            
            if (user == null)
            {
                // Try to find by username only to debug
                var userByName = await _dashGen2026Context.Users.FirstOrDefaultAsync(x => x.UserName == loginUser.UserName);
                if (userByName != null)
                {
                    Console.WriteLine($"User found by name: {userByName.UserName}, but password mismatch. Expected: '{loginUser.Password}', Got: '{userByName.Password}'");
                }
                else
                {
                    Console.WriteLine($"User not found with username: {loginUser.UserName}");
                }
            }
            
            return user;

        }
        public async Task UpdateUser(int id, User user)
        {
            var existingUser = await _dashGen2026Context.Users.FindAsync(id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.IsAdmin = user.IsAdmin;
                await _dashGen2026Context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _dashGen2026Context.Users.ToListAsync(); ;
        }


        //public async Task DeleteUser(int id)
        //{
        //    var user = await _dashGen2026Context.Users.FindAsync(id);
        //    if (user != null)
        //    {
        //        _dashGen2026Context.Users.Remove(user);
        //        await _dashGen2026Context.SaveChangesAsync();
        //    }
        //}

        public async Task<bool> DeleteUser(int id, int adminId)
        {
            var performingUser = await _dashGen2026Context.Users.FindAsync(adminId);

            if (performingUser == null || !performingUser.IsAdmin)
            {
                return false;
            }

            var userToDelete = await _dashGen2026Context.Users.FindAsync(id);
            if (userToDelete != null)
            {
                _dashGen2026Context.Users.Remove(userToDelete);
                await _dashGen2026Context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }

}
