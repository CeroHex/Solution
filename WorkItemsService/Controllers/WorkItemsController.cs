using Microsoft.AspNetCore.Mvc;
using WorkItemsService.Models;
using WorkItemsService.Services;


namespace WorkItemsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkItemsController : ControllerBase
    {

        private readonly WorkItemService _workItemService;


        public WorkItemsController(
            WorkItemService service)
        {
            _workItemService = service;
        }

        #region CREATE ITEM
        [HttpPost]
        public async Task<IActionResult> createItem(
            WorkItem item)
        {
            var result = await _workItemService.createItem(item);

            return Ok(result);
        }
        #endregion

        #region READ ITEMS
        [HttpGet]
        public async Task<IActionResult> getItems()
        {
            var items = await _workItemService.getItems();

            return Ok(items);
        }
        #endregion

        #region DELETE ITEM
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteItem(Guid id)
        {
            var result = await _workItemService.deleteItem(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
        #endregion
    }
}