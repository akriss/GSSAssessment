using GSSAssessment.Common.DataSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.SyncJob
{
    internal class SyncManager
    {
        public void Sync()
        {
            new BinSync().Sync();
            new ItemSync().Sync();
        }

        public void SendTransactions()
        {
            new ItemSync().SendAllItemUpdates();
            new ItemSync().SendAllItemRemovals();
        }
    }
}
