using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSSAssessment.Common.Models
{
    public class ItemQuantity
    {
        public int? Id { get; set; }
        public int ItemId { get; set; }
        public int BinId { get; set; }
        public int Quantity { get; set; }
    }
}
