using GSSAssessment.Common.Database;
using GSSAssessment.Common.Models;
using GSSAssessment.Common.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.DataManagers
{
    public static class ItemManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Text of error message.  Empty string if no error<</returns>
        public static string AddOrUpdate(Item item, bool fromExternalSystem)
        {
            if (item == null)
                return "item cannot be null";

            try
            {
                using (var context = DatabaseContextFactory.GetDatabaseContext())
                {
                    context.AddOrUpdateModel<Item>(item);

                    if (!fromExternalSystem)
                    {
                        var transaction = new ItemUpdateTransaction()
                        {
                            Description = item.Description,
                            ItemId = item.Id
                        };
                        context.AddOrUpdateModel(transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error attempting to add or update item: {item.Id}";
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Text of error message.  Empty string if no error</returns>
        public static string RemoveItem(Item item, bool fromExternalSystem)
        {
            if (item == null)
                return "Item cannot be null";

            try
            {
                using (var context = DatabaseContextFactory.GetDatabaseContext())
                {
                    context.RemoveModel<Item>(item.Id);

                    if(!fromExternalSystem)
                    {
                        var transaction = new ItemRemovalTransaction()
                        {
                            ItemId = item.Id
                        };
                        context.AddOrUpdateModel(transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error attempting to remove item: {item.Id}";
            }

            return string.Empty;
        }
    }
}
