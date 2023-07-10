using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.Database
{
    public class DatabaseContext : IDatabaseContext
    {
        public void AddOrUpdateModel<T>(T model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void InitDb()
        {
            throw new NotImplementedException();
        }

        public List<T> LoadModels<T>(Func<T, bool>? parameters = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveModel<T>(int id)
        {
            throw new NotImplementedException();
        }
    }
}
