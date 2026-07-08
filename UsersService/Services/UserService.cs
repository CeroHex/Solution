using UsersService.Models;
using UsersService.Repositories;

namespace UsersService.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        #region GET USERS
        public async Task<List<User>> getUsers()
        {
            return await _userRepository.getUsers();
        }
        #endregion

        #region CREATE USER
        public async Task<User> createUser(User user)
        {
            return await _userRepository.createUser(user);
        }
        #endregion

        #region DELETE USER
        public async Task<bool> deleteUser(Guid id)
        {
            return await _userRepository.deleteUser(id);
        }
        #endregion
    }
}