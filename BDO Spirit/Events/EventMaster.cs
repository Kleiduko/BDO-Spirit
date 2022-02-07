using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDO_Spirit.Events
{
    public class EventMaster
    {
        #region Change Observable
        public delegate void ChangeObservable(ChangeObservableEventArgs args);
        public static event ChangeObservable OnChangeObservable;

        public static void CallOnChangeObservable(ChangeObservableEventArgs args)
        {
            OnChangeObservable?.Invoke(args);
        }
        #endregion

        #region Refresh Observables
        public delegate void RefreshObservables(ObservablesRefreshEventArgs args);
        public static event RefreshObservables OnRefreshObservables;

        public static void CallOnRefreshObservables(ObservablesRefreshEventArgs args)
        {
            OnRefreshObservables?.Invoke(args);
        }
        #endregion
    }
}
