using GSSAssessment.Common.Models;
using GSSAssessment.Common.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.Database.JsonTestDb
{
    public class JsonObjectMap : IDisposable
    {
        public JsonObjectMap()
        {
            Bins = new List<Bin>();
            Items = new List<Item>();
            ItemQuantities = new List<ItemQuantity>();

            ItemUpdateTransactions = new List<ItemUpdateTransaction>();
            ItemRemovalTransactions = new List<ItemRemovalTransaction>();
        }

        public List<Bin> Bins { get; set; }
        public List<Item> Items { get; set; }
        public List<ItemQuantity> ItemQuantities { get; set; }

        public List<ItemUpdateTransaction> ItemUpdateTransactions { get; set; }
        public List<ItemRemovalTransaction> ItemRemovalTransactions { get; set; }

        public void Dispose()
        {
            Bins = null;
            Items = null;
            ItemQuantities = null;

            ItemUpdateTransactions = null;
            ItemRemovalTransactions = null;
        }
    }
}
