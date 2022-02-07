using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDO_Spirit.Api.Models.BDOMarket
{
    public class MarketIdSerachItem
    {
        public string name { get; set; }
        public int id { get; set; }
        public long price { get; set; }
        public long count { get; set; }
        public int enhanceGrade { get; set; }
        public long totalTrades { get; set; }
        public string icon { get; set; }
        public int grade { get; set; }
    }
}
