using GSSAssessment.Common.Database;
using GSSAssessment.Common.DataManagers;
using GSSAssessment.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSSAssessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        [HttpGet("GetItems")]
        public IEnumerable<Item> GetItems()
        {
            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var items = context.LoadModels<Item>();

                return items;
            }
        }

        [HttpGet("GetItem/{itemId}")]
        public ActionResult<Item> GetItem(int itemId)
        {
            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {

                var item = context.LoadModels<Item>(x => x.Id == itemId).FirstOrDefault();
                
                if(item == null)
                {
                    return NotFound();
                }

                return item;
            }
        }

        [HttpGet("GetItemAssignments/{itemId}")]
        public ActionResult<List<ItemQuantity>> GetItemAssignments(int itemId)
        {
            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var itemAssignments = context.LoadModels<ItemQuantity>(x => x.ItemId == itemId);

                return itemAssignments;
            }
        }

        [HttpPost("AssignItem")]
        public ActionResult AssignItem(int binId, int itemId)
        {
            var result = ItemAssignmentManager.AssignItemToBin(binId, itemId);

            if (result != string.Empty)
                return StatusCode(500, result);
            else
                return StatusCode(200);
        }

        [HttpPost("RemoveFromBin")]
        public ActionResult RemoveFromBin(int binId, int itemId)
        {
            var result = ItemAssignmentManager.RemoveItemFromBin(binId, itemId);

            if (result != string.Empty)
                return StatusCode(500, result);
            else
                return StatusCode(200);
        }


        [HttpPost("UpdateItemQuantity")]
        public ActionResult UpdateItemQuantity(int binId, int itemId, int quantity)
        {
            var result = ItemAssignmentManager.UpdateItemQtyInBin(binId, itemId, quantity);

            if (result != string.Empty)
                return StatusCode(500, result);
            else
                return StatusCode(200);
        }
    }
}