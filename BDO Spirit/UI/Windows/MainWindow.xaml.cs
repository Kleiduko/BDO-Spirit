using BDO_Spirit.Api;
using BDO_Spirit.Events;
using BDO_Spirit.Models;
using BDO_Spirit.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using WPFUI.Controls;

namespace BDO_Spirit
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<ObservableModel> Observables { get; } = new List<ObservableModel>();

        private Timer timer;

        public MainWindow()
        {
            WPFUI.Background.Manager.Apply(this);

            InitializeComponent();

            LoadPriceAlert();

            timer = new Timer();
            timer.Interval = 1000 * 60 * 30;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;

            Timer_Elapsed(null, null);

            EventMaster.OnChangeObservable += EventMaster_OnChangeObservable;
        }

        private void EventMaster_OnChangeObservable(ChangeObservableEventArgs args)
        {
            if (args.ChangeType == ChangeType.Add)
            {
                Observables.Add(args.ObservableModel);
                LoadObservable(args.ObservableModel);
            }
            else
            {
                Observables.Remove(args.ObservableModel);
            }
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await LoadObservables(Observables);


            foreach (ObservableModel model in Observables)
            {
                new ItemIconService(model.BulkItemSearch.name).SaveImage(model.BulkItemSearch.icon);
                if (model.PriceAlert > model.BulkItemSearch.price)
                {
                    var toast = new ToastContentBuilder()
                        .AddText(model.BulkItemSearch.name)
                        .AddText($"This item has fallen below the price you set in the Central Market. There are {model.BulkItemSearch.count} items available.")
                        .AddAttributionText($"Alert price: {model.PriceAlert} / Current price {model.BulkItemSearch.price}")
                        .AddCustomTimeStamp(DateTime.Now);

                    var icon = ItemIconService.GetIconPath(model.BulkItemSearch.name);

                    if(icon != null)
                    {
                        toast.AddAppLogoOverride(new Uri(icon), ToastGenericAppLogoCrop.Circle);
                    }

                    toast.Show();
                }
            }
        }

        public async Task LoadObservables(List<ObservableModel> models)
        {
            var list = new List<int>();

            models.ForEach(x => list.Add(x.Id));

            var result = await BDOMarket.BulkSearchItemsById(list.ToArray());

            foreach (var item in result)
            {
                var old = Observables.Find(x => x.Id == item.id);

                if (old == null)
                {
                    continue;
                }

                old.BulkItemSearch = item;
            }

            var eventArgs = new ObservablesRefreshEventArgs();
            eventArgs.ObservableModels = Observables;

            EventMaster.CallOnRefreshObservables(eventArgs);
        }

        public async void LoadObservable(ObservableModel model)
        {
            var result = await BDOMarket.BulkSearchItemsById(new int[] { model.Id });

            foreach (var item in result)
            {
                var old = Observables.Find(x => x.Id == item.id);

                if (old == null)
                {
                    continue;
                }

                old.BulkItemSearch = item;
            }

            var eventArgs = new ObservablesRefreshEventArgs();
            eventArgs.ObservableModels = Observables;

            EventMaster.CallOnRefreshObservables(eventArgs);
        }

        private void LoadPriceAlert()
        {
            var PATH = AppDomain.CurrentDomain.BaseDirectory + "Observables.txt";

            if (!File.Exists(PATH))
            {
                var file = File.Create(PATH);
                file.Close();
            }

            var fileContent = File.ReadAllText(PATH);

            var observables = JsonConvert.DeserializeObject<List<ObservableModel>>(fileContent);

            if (observables != null)
            {
                Observables.AddRange(observables);
            }
        }

        private void NavigationFluent_Loaded(object sender, RoutedEventArgs e)
        {
            var nav = sender as NavigationFluent;
            nav.Navigate("news");
        }

        private void NavigationFluent_Navigated(object sender, RoutedEventArgs e)
        {

        }
    }
}
