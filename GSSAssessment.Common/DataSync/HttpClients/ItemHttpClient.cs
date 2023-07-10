using GSSAssessment.Common.Models;
using GSSAssessment.Common.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GSSAssessment.Common.DataSync.HttpClients
{
    public class ItemHttpClient
    {
        public string GetJsonResponse()
        {
            //This is just mocked up.  Obviously it will need to be expanded properly to actually work
            var item1 = new Item()
            {
                Id = 100,
                Description = "Item100"
            };
            var item2 = new Item()
            {
                Id = 200,
                Description = "Item200"
            };
            var item3 = new Item()
            {
                Id = 300,
                Description = "Item300"
            };

            var items = new List<Item>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            var result = JsonSerializer.Serialize(items);

            return result;
        }

        public bool SendItemUpdate(ItemUpdateTransaction transaction)
        {
            return true;
        }

        public bool SendItemRemoval(ItemRemovalTransaction transaction)
        {
            return true;
        }
    }
}
