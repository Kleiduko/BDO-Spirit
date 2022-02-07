using BDO_Spirit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDO_Spirit.Events
{
    public class ChangeObservableEventArgs : EventArgs
    {
        public ObservableModel ObservableModel { get; set; }
        public ChangeType ChangeType { get; set; }
    }

    public enum ChangeType
    {
        Add,
        Remove
    }
}
