using BDO_Spirit.Api;
using BDO_Spirit.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDO_Spirit.Services
{
    public class ItemsObservableService
    {
        string observablesPath = AppDomain.CurrentDomain.BaseDirectory + "Files\\NotifyItems.txt";

        private static List<ObservableModel> Observables;

        private static BDOMarket Market;

        public ItemsObservableService()
        {
            Market = new BDOMarket();

            
        }

        public void StartService()
        {

        }
    
        private void LoadObservables()
        {
            
        }

        private void CreateFileAndDirectory()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory + "Files";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }


        }
    }
}
