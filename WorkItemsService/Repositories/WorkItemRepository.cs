using System.Text.Json;
using WorkItemsService.Models;

namespace WorkItemsService.Repositories
{
    public class WorkItemRepository
    {
        private readonly IConfiguration _configuration;


        public WorkItemRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #region GET ITEMS
        public async Task<List<WorkItem>> getItems()
        {
            var path = _configuration["DataFiles:WorkItems"];


            if (!File.Exists(path))
            {
                return new List<WorkItem>();
            }


            var json = await File.ReadAllTextAsync(path);


            return JsonSerializer.Deserialize<List<WorkItem>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<WorkItem>();
        }
        #endregion

        #region CREATE ITEM
        public async Task<WorkItem> createItem(WorkItem item)
        {
            var items = await getItems();


            item.Id = Guid.NewGuid();


            items.Add(item);


            var path = _configuration["DataFiles:WorkItems"];


            await File.WriteAllTextAsync(
                path!,
                JsonSerializer.Serialize(items,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                })
            );


            return item;
        }
        #endregion

        #region DELETE ITEM
        public async Task<bool> deleteItem(Guid id)
        {
            var path = _configuration["DataFiles:WorkItems"];


            if (!File.Exists(path))
            {
                return false;
            }


            var json = await File.ReadAllTextAsync(path);


            var items = JsonSerializer.Deserialize<List<WorkItem>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<WorkItem>();


            var item = items.FirstOrDefault(x => x.Id == id);


            if (item == null)
            {
                return false;
            }


            items.Remove(item);


            var newJson = JsonSerializer.Serialize(
                items,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );


            await File.WriteAllTextAsync(path, newJson);


            return true;
        }
        #endregion
    }
}