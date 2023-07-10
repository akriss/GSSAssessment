using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.Models.Transactions
{
    public class ItemUpdateTransaction
    {
        public int? Id { get; set; }
        public int? ItemId { get; set; }
        public string Description { get; set; }
    }
}
