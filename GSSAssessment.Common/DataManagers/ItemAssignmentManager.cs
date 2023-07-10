using GSSAssessment.Common.Database;
using GSSAssessment.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.DataManagers
{
    public class ItemAssignmentManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="item"></param>
        /// <returns>Text of error message.  Empty string if no error</returns>
        public static string AssignItemToBin(Bin bin, Item item)
        {
            if (bin == null)
                return "bin cannot be null";
            if (item == null)
                return "Item cannot be null";

            return AssignItemToBin(bin.Id, item.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binId"></param>
        /// <param name="itemId"></param>
        /// <returns>Text of error message.  Empty string if no error</returns>
        public static string AssignItemToBin(int binId, int itemId)
        {
            try
            {
                using (IDatabaseContext context = DatabaseContextFactory.GetDatabaseContext())
                {
                    var existingQtys = context.LoadModels<ItemQuantity>(x => x.BinId == binId && x.ItemId == itemId);

                    if (existingQtys.Count > 0)
                    {
                        return $"Item {itemId} already assigned to bin {binId}";
                    }

                    var qtyAssignment = new ItemQuantity()
                    {
                        ItemId = itemId,
                        BinId = binId,
                        Quantity = 0
                    };

                    context.AddOrUpdateModel(qtyAssignment);
                }
            }
            catch (Exception ex)
            {
                return $"Error attempting to add item {itemId} to bin: {binId}";
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="item"></param>
        /// <returns>Text of error message.  Empty string if no error</returns>
        public static string RemoveItemFromBin(Bin bin, Item item)
        {
            if (bin == null)
                return "bin cannot be null";
            if (item == null)
                return "Item cannot be null";

            return RemoveItemFromBin(bin.Id, item.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binId"></param>
        /// <param name="itemId"></param>
        /// <returns>Text of error message.  Empty string if no error</returns>
        public static string RemoveItemFromBin(int binId, int itemId)
        {
            try
            {
                using (IDatabaseContext context = DatabaseContextFactory.GetDatabaseContext())
                {
                    var existingQtys = context.LoadModels<ItemQuantity>(x => x.BinId == binId && x.ItemId == itemId);

                    if (existingQtys == null)
                    {
                        return $"Item {itemId} not previously assigned to bin {binId}";
                    }

                    foreach (var existing in existingQtys)
                        context.RemoveModel<ItemQuantity>(existing.Id.Value);
                }
            }
            catch (Exception ex)
            {
                return $"Error attempting to remove item {itemId} from bin {binId}";
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="item"></param>
        /// <returns>Text of error message.  Empty string if no error</returns>
        public static string UpdateItemQtyInBin(Bin bin, Item item, int quantity)
        {
            if (bin == null)
                return "bin cannot be null";
            if (item == null)
                return "Item cannot be null";

            return UpdateItemQtyInBin(bin.Id, item.Id, quantity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="item"></param>
        /// <returns>Text of error message.  Empty string if no error</returns>
        public static string UpdateItemQtyInBin(int binId, int itemId, int quantity)
        {
            if (quantity < 0)
                return "Total quantity must be non-negative";

            try
            {
                using (IDatabaseContext context = DatabaseContextFactory.GetDatabaseContext())
                {
                    var existingQtys = context.LoadModels<ItemQuantity>(x => x.BinId == binId && x.ItemId == itemId);

                    if (existingQtys.Count == 0)
                    {
                        return $"Item {itemId} not assigned to bin {binId}";
                    }

                    var qty = existingQtys.First();

                    qty.Quantity = quantity;

                    context.AddOrUpdateModel(qty);
                }
            }
            catch (Exception ex)
            {
                return $"Error attempting to update quantity of item {itemId} in bin {binId}";
            }

            return string.Empty;
        }
    }
}
