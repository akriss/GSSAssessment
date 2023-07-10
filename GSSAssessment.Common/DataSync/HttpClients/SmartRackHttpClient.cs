using GSSAssessment.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GSSAssessment.Common.DataSync.HttpClients
{
    public class SmartRackHttpClient
    {
        public string GetJsonResponse()
        {
            //This is just mocked up.  Obviously it will need to be expanded properly to actually work
            var bin1 = new Bin()
            {
                Id = 100,
                Description = "Bin100"
            };
            var bin2 = new Bin()
            {
                Id = 200,
                Description = "Bin200"
            };
            var bin3 = new Bin()
            {
                Id = 300,
                Description = "Bin300"
            };

            var bins = new List<Bin>();
            bins.Add(bin1);
            bins.Add(bin2);
            bins.Add(bin3);

            var result = JsonSerializer.Serialize(bins);

            return result;
        }
    }
}
