using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDO_Spirit.Models
{
    public class ObservableModel
    {
        public int Id { get; set; }
        public long PriceAlert { get; set; }

        [JsonIgnore]
        public BulkItemSearch BulkItemSearch { get; set; }
    }
}
