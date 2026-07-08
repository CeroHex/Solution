using System.Text.Json;
using WorkItemsService.DTOs;

namespace WorkItemsService.Services
{
    public class UserApiService
    {
        private readonly HttpClient _client;


        public UserApiService(
            IHttpClientFactory factory)
        {
            _client = factory.CreateClient("users");
        }


        public async Task<List<UserDto>> GetUsers()
        {
            var response =
                await _client.GetAsync("/api/users");


            response.EnsureSuccessStatusCode();


            var json =
                await response.Content.ReadAsStringAsync();


            return JsonSerializer.Deserialize<List<UserDto>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<UserDto>();
        }
    }
}