using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.Database
{
    public static class DatabaseContextFactory
    {
        private static Type _dbContextType { get; set; }

        public static void Init(Type dbContextType)
        {
            if (!dbContextType.GetInterfaces().Contains(typeof(IDatabaseContext)))
                throw new Exception("Type must use the interface IDabaseContext");

            _dbContextType = dbContextType;
        }

        public static IDatabaseContext GetDatabaseContext()
        {
            if (_dbContextType == null)
                throw new Exception("Must register a DB Context by calling Init before GetDatabaseContext can be called");
            return Activator.CreateInstance(_dbContextType) as IDatabaseContext;
        }
    }
}
