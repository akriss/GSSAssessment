using GSSAssessment.Common.Database;
using GSSAssessment.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.DataManagers
{
    public static class BinManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bin"></param>
        /// <returns>Text of error message.  Empty string if no error</returns>
        public static string AddOrUpdate(Bin bin)
        {
            if (bin == null)
                return "bin cannot be null";

            try
            {
                using (var context = DatabaseContextFactory.GetDatabaseContext())
                {
                    context.AddOrUpdateModel<Bin>(bin);
                }
            }
            catch(Exception ex)
            {
                return $"Error attempting to add or update bin: {bin.Id}";
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bin"></param>
        /// <returns>Text of error message.  Empty string if no error</returns>
        public static string RemoveBin(Bin bin)
        {
            if (bin == null)
                return "bin cannot be null";

            try
            {
                using (var context = DatabaseContextFactory.GetDatabaseContext())
                {
                    context.RemoveModel<Bin>(bin.Id);
                }
            }
            catch(Exception ex)
            {
                return $"Error attempting to remove bin: {bin.Id}";
            }

            return string.Empty;
        }
    }
}
