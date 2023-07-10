using GSSAssessment.Common.Database;
using GSSAssessment.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSSAssessment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BinController : Controller
    {
        [HttpGet("GetBins")]
        public IEnumerable<Bin> GetBins()
        {
            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var bins = context.LoadModels<Bin>();

                return bins;
            }
        }

        [HttpGet("GetBin/{binId}")]
        public ActionResult<Bin> GetBin(int binId)
        {
            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var bin = context.LoadModels<Bin>(x => x.Id == binId).FirstOrDefault();

                if (bin == null)
                {
                    return NotFound();
                }

                return bin;
            }
        }

        [HttpGet("GetItemsInBin/{binId}")]
        public ActionResult<List<Item>> GetItemsInBin(int binId)
        {
            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var itemAssignments = context.LoadModels<ItemQuantity>(x => x.BinId == binId);

                var items = context.LoadModels<Item>(x => itemAssignments.Any(y => y.ItemId == x.Id));

                return items;
            }
        }

        [HttpGet("GetItemQuantitiesInBin/{binId}")]
        public ActionResult<List<ItemQuantity>> GetItemQuantitiesInBin(int binId)
        {
            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                var itemAssignments = context.LoadModels<ItemQuantity>(x => x.BinId == binId);

                return itemAssignments;
            }
        }
    }
}
