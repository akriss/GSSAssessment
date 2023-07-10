using GSSAssessment.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GSSAssessment.Common.DataSync
{
    public abstract class ISync<T> where T : new()
    {
        public abstract void Sync();

        protected void SaveJsonResultToDb(string json)
        {
            var results = JsonSerializer.Deserialize<List<T>>(json);

            using(var context = DatabaseContextFactory.GetDatabaseContext())
            {
                foreach (var record in results)
                {
                    context.AddOrUpdateModel(record);
                }
            }
        }
    }
}
