using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDO_Spirit.Api.Models.BDOMarket
{
    public class MarketNameSearch
    {
        public List<MarketNameSearchItem> list { get; set; }

        public int resultCode { get; set; }
        public string resultMsg { get; set; }
    }
}
