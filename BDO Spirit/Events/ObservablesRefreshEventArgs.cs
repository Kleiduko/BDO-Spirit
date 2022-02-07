using BDO_Spirit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDO_Spirit.Events
{
    public class ObservablesRefreshEventArgs : EventArgs
    {
        public List<ObservableModel> ObservableModels { get; set; }
    }
}
