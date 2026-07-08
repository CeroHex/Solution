using WorkItemsService.DTOs;
using WorkItemsService.Models;
using WorkItemsService.Repositories;

namespace WorkItemsService.Services
{
    public class WorkItemService
    {
        private readonly WorkItemRepository _workItemRepository;
        private readonly UserApiService _userApi;


        public WorkItemService(WorkItemRepository workItemRepository, UserApiService userApi)
        {
            _workItemRepository = workItemRepository;
            _userApi = userApi;
        }

        #region CREATE ITEM
        public async Task<WorkItem> createItem(WorkItem item)
        {
            var users = await _userApi.GetUsers();

            var items = await _workItemRepository.getItems();


            var days =
                (item.DueDate.Date - DateTime.Now.Date).Days;


            UserDto selectedUser;


            // REGLA 1:
            // Próximo a vencer: menos de 3 días
            // Ignora relevancia

            if (days < 3)
            {
                selectedUser = users
                    .OrderBy(u =>
                        items.Count(i =>
                            i.AssignedUserId == u.Id))
                    .First();
            }


            // REGLA 2:
            // Alta relevancia

            else if (item.Priority == "HIGH")
            {
                selectedUser = users
                    .Where(u =>
                    {
                        var highPending =
                            items.Count(i =>
                                i.AssignedUserId == u.Id &&
                                i.Priority == "HIGH" &&
                                i.Status == "PENDING");


                        // saturado
                        return highPending <= 3;

                    })
                    .OrderBy(u =>
                        items.Count(i =>
                            i.AssignedUserId == u.Id &&
                            i.Status == "PENDING"))
                    .First();
            }


            // REGLA 3:
            // Baja relevancia

            else
            {
                selectedUser = users
                    .OrderBy(u =>
                        items.Count(i =>
                            i.AssignedUserId == u.Id &&
                            i.Status == "PENDING"))
                    .First();
            }


            item.Id = Guid.NewGuid();

            item.AssignedUserId = selectedUser.Id;

            item.AssignedUsername = selectedUser.Username;

            item.Status = "PENDING";


            return await _workItemRepository.createItem(item);
        }
        #endregion

        #region READ ITEMS
        public async Task<List<WorkItem>> getItems()
        {
            return await _workItemRepository.getItems();
        }
        #endregion

        #region DELETE ITEM
        public async Task<bool> deleteItem(Guid id)
        {
            return await _workItemRepository.deleteItem(id);
        }
        #endregion
    }
}