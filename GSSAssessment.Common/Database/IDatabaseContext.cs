using GSSAssessment.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.Database
{
    public interface IDatabaseContext : IDisposable
    {
        public List<T> LoadModels<T>(Func<T, bool>? parameters = null);

        public void AddOrUpdateModel<T>(T model);

        public void RemoveModel<T>(int id);

        public void InitDb();
    }
}
