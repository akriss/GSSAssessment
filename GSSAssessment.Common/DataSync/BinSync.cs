using GSSAssessment.Common.DataSync.HttpClients;
using GSSAssessment.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.DataSync
{
    public class BinSync : ISync<Bin>
    {
        public override void Sync()
        {
            var jsonResult = (new SmartRackHttpClient()).GetJsonResponse();

            SaveJsonResultToDb(jsonResult);
        }
    }
}
