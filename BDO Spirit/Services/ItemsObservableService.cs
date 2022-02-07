using BDO_Spirit.Api;
using BDO_Spirit.Events;
using BDO_Spirit.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;

namespace BDO_Spirit.Services
{
    public class ItemsObservableService
    {
        string observablesPath = AppDomain.CurrentDomain.BaseDirectory + "Files\\NotifyItems.txt";

        public List<ObservableModel> Observables = new List<ObservableModel>();

        private static BDOMarket Market;

        private static Timer UpdateTimer;

        public ItemsObservableService()
        {
            Market = new BDOMarket();

            CreateFileAndDirectory();

            EventMaster.OnChangeObservable += ObservablesChanged;
        }

        public void StartService()
        {
            LoadObservables();

            CreateTimer();

            UpdateTimer.Start();

            UpdateObservabels(null, null);
        }

        public void StopService()
        {
            UpdateTimer.Stop();
        }

        private void CreateTimer()
        {
            UpdateTimer = new Timer();

            UpdateTimer.Interval = 1000 * 60 * 30;
            UpdateTimer.Elapsed += UpdateObservabels;
            UpdateTimer.AutoReset = true;
        }

        private void UpdateObservabels(object sender, ElapsedEventArgs e)
        {
            List<int> ids = new List<int>();

            Observables.ForEach(i => ids.Add(i.Id));

            LoadObservablesData(ids);
        }

        private void ObservablesChanged(ChangeObservableEventArgs args)
        {
            if (args.ChangeType == ChangeType.Add)
            {
                Observables.Add(args.ObservableModel);

                LoadObservablesData(new int[] { args.ObservableModel.Id }.ToList());
            }
            else if (args.ChangeType == ChangeType.Remove)
            {
                Observables.Remove(args.ObservableModel);
            }else if(args.ChangeType == ChangeType.Modify)
            {
                //Only save file
            }

            SaveObservables();
        }

        private async void LoadObservablesData(List<int> ids)
        {
            var result = await Market.BulkSearchItemsById(ids.ToArray());

            foreach (var item in result)
            {
                var observable = Observables.Find(x => x.Id == item.id);

                if (observable != null)
                {
                    observable.BulkItemSearch = item;
                }

                if (observable.PriceAlert > item.price)
                {
                    SendAlertToast(observable);
                }
            }

            var eventArgs = new ObservablesRefreshEventArgs()
            {
                ObservableModels = Observables
            };

            EventMaster.CallOnRefreshObservables(eventArgs);
        }

        private void SendAlertToast(ObservableModel model)
        {
            ItemIconService iconService = new ItemIconService(model.BulkItemSearch.name);
            iconService.SaveImage(model.BulkItemSearch.icon);

            var toast = new ToastContentBuilder()
                .AddText(model.BulkItemSearch.name)
                .AddText($"This item has fallen below the price you set in the Central Market. There are {model.BulkItemSearch.count} items available.")
                .AddAttributionText($"Alert price: {model.PriceAlert} / Current price {model.BulkItemSearch.price}");

            var icon = iconService.GetIconPath();

            if (icon != null)
            {
                toast.AddAppLogoOverride(new Uri(icon), ToastGenericAppLogoCrop.Circle);
            }

            toast.SetToastScenario(ToastScenario.Reminder);

            toast.Show();
        }

        private void LoadObservables()
        {
            string json = File.ReadAllText(observablesPath);

            var loadedObservables = JsonConvert.DeserializeObject<List<ObservableModel>>(json);

            if(loadedObservables != null)
            {
                Observables.AddRange(loadedObservables);
            }
        }

        private void SaveObservables()
        {
            string json = JsonConvert.SerializeObject(Observables);

            File.WriteAllText(observablesPath, json);
        }

        private void CreateFileAndDirectory()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory + "Files";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(observablesPath))
            {
                var stream = File.Create(observablesPath);

                stream.Close();
            }
        }
    }
}
