using System.Text.Json;
using UsersService.Models;

namespace UsersService.Repositories
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #region GET USERS
        public async Task<List<User>> getUsers()
        {
            var path = _configuration["DataFiles:Users"];

            var json = await File.ReadAllTextAsync(path!);

            return JsonSerializer.Deserialize<List<User>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<User>();
        }
        #endregion 

        #region CREATE USER
        public async Task<User> createUser(User user)
        {
            var path = _configuration["DataFiles:Users"];

            var json = await File.ReadAllTextAsync(path!);

            var users = JsonSerializer.Deserialize<List<User>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<User>();


            user.Id = Guid.NewGuid();

            users.Add(user);


            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var newJson = JsonSerializer.Serialize(users, options);


            await File.WriteAllTextAsync(path!, newJson);


            return user;
        }
        #endregion

        #region DELETE USER
        public async Task<bool> deleteUser(Guid id)
        {
            var path = _configuration["DataFiles:Users"];

            var json = await File.ReadAllTextAsync(path!);

            var users = JsonSerializer.Deserialize<List<User>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<User>();


            var user = users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return false;


            users.Remove(user);


            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };


            var newJson = JsonSerializer.Serialize(users, options);


            await File.WriteAllTextAsync(path!, newJson);


            return true;
        }
        #endregion
    }
}