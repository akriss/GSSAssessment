using GSSAssessment.Common.Database;
using GSSAssessment.Common.DataSync.HttpClients;
using GSSAssessment.Common.Models;
using GSSAssessment.Common.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.DataSync
{
    public class ItemSync : ISync<Item>
    {
        public override void Sync()
        {
            var jsonResult = (new ItemHttpClient()).GetJsonResponse();

            SaveJsonResultToDb(jsonResult);
        }

        public void SendAllItemRemovals()
        {
            List<ItemRemovalTransaction> transactions;

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                transactions = context.LoadModels<ItemRemovalTransaction>();
            }

            foreach (var transaction in transactions)
            {
                ItemRemoval(transaction);
            }
        }

        public void ItemRemoval(ItemRemovalTransaction transaction)
        {
            var success = (new ItemHttpClient()).SendItemRemoval(transaction);
            if(success)
            {
                using (var context = DatabaseContextFactory.GetDatabaseContext())
                {
                    context.RemoveModel<ItemRemovalTransaction>(transaction.Id);
                }
            }
        }

        public void SendAllItemUpdates()
        {
            List<ItemUpdateTransaction> transactions;

            using (var context = DatabaseContextFactory.GetDatabaseContext())
            {
                transactions = context.LoadModels<ItemUpdateTransaction>();
            }

            foreach (var transaction in transactions)
            {
                ItemUpdate(transaction);
            }
        }

        public void ItemUpdate(ItemUpdateTransaction transaction)
        {
            var success = (new ItemHttpClient()).SendItemUpdate(transaction);
            if (success)
            {
                using (var context = DatabaseContextFactory.GetDatabaseContext())
                {
                    context.RemoveModel<ItemUpdateTransaction>(transaction.Id.Value);
                }
            }
        }
    }
}
