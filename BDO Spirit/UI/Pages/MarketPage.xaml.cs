using BDO_Spirit.Api;
using BDO_Spirit.Api.Models.BDOMarket;
using BDO_Spirit.Events;
using BDO_Spirit.Models;
using BDO_Spirit.UI.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WPFUI.Common;
using WPFUI.Controls;
using WPFUI.Controls.Interfaces;

namespace BDO_Spirit.UI.Pages
{
    /// <summary>
    /// Interaktionslogik für MarketPage.xaml
    /// </summary>
    public partial class MarketPage : Page
    {
        private string PATH { get; }

        private List<ObservableModel> ObservableModels = new List<ObservableModel>();

        public MarketPage()
        {
            PATH = AppDomain.CurrentDomain.BaseDirectory + "Observables.txt";

            InitializeComponent();

            var window = Window.GetWindow(this) as MainWindow;

            ObservableModels.AddRange(window.Observables);

            EventManager.RegisterClassHandler(typeof(Grid), Grid.MouseLeftButtonDownEvent, new RoutedEventHandler(this.ItemsControlClick));
            EventManager.RegisterClassHandler(typeof(WPFUI.Controls.Icon), WPFUI.Controls.Icon.MouseLeftButtonDownEvent, new RoutedEventHandler(this.HandleObservables));
        }

        private void HandleObservables(object sender, RoutedEventArgs e)
        {
            var icon = sender as WPFUI.Controls.Icon;

            var item = GetClickedItemByParent(sender);

            if (item == null)
            {
                return;
            }

            var observableItem = ObservableModels.Find(c => c.Id == item.mainKey);

            if (observableItem != null)
            {
                ObservableModels.Remove(observableItem);
                item.star = WPFUI.Common.Icon.StarAdd24;

                var eventArgs = new ChangeObservableEventArgs();
                eventArgs.ObservableModel = observableItem;
                eventArgs.ChangeType = ChangeType.Remove;
                EventMaster.CallOnChangeObservable(eventArgs);
            }
            else
            {
                if (ObservableModels.Count >= 10)
                {
                    Snack.Title = "Observable items limit reached";
                    Snack.Content = "Observable items has a limit of 10!";
                    Snack.Expand();
                    return;
                }

                var model = new ObservableModel() { Id = item.mainKey, PriceAlert = item.totalSumCount };
                ObservableModels.Add(model);
                item.star = WPFUI.Common.Icon.Star24;

                var eventArgs = new ChangeObservableEventArgs();
                eventArgs.ObservableModel = model;
                eventArgs.ChangeType = ChangeType.Add;
                EventMaster.CallOnChangeObservable(eventArgs);
            }

            SaveCurrentObservabels();

            ItemsControl.Items.Refresh();
        }


        private void SaveCurrentObservabels()
        {
            var json = JsonConvert.SerializeObject(ObservableModels);

            File.WriteAllText(PATH, json);
        }

        private void ItemsControlClick(object sender, RoutedEventArgs e)
        {
            var item = GetClickedItemByChildren(sender);

            if (item == null)
            {
                return;
            }
            Debug.WriteLine("market");
            //Opens market window
        }

        private MarketNameSearchItem GetClickedItemByChildren(object sender)
        {
            var grid = sender as Grid;

            if (grid == null || grid.Name != "ItemGrid")
            {
                return null;
            }

            var border = grid.Children[1] as Border;

            var textBlock = border.Child as TextBlock;

            var itemName = textBlock.Text;

            foreach (var item in ItemsControl.Items)
            {
                var marketItem = item as MarketNameSearchItem;

                if (marketItem.name != itemName)
                {
                    continue;
                }

                return marketItem;
            }
            return null;
        }

        private MarketNameSearchItem GetClickedItemByParent(object sender)
        {
            var icon = sender as WPFUI.Controls.Icon;

            var rootGrid = icon.Parent as Grid;

            if (rootGrid == null)
            {
                rootGrid = icon.Parent as Grid;
                return null;
            }

            var grid = rootGrid.Children[0] as Grid;

            if (grid == null || grid.Name != "ItemGrid")
            {
                return null;
            }

            var border = grid.Children[1] as Border;

            var textBlock = border.Child as TextBlock;

            var itemName = textBlock.Text;

            foreach (var item in ItemsControl.Items)
            {
                var marketItem = item as MarketNameSearchItem;

                if (marketItem.name != itemName)
                {
                    continue;
                }

                return marketItem;
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new PriceAlertWindow().Show();
        }

        private async void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var textBox = sender as TextBox;

            if (string.IsNullOrEmpty(textBox.Text))
            {
                return;
            }

            ItemsControl.Items.Clear();

            try
            {
                var id = Convert.ToInt32(textBox.Text);

                var items = await BDOMarket.SearchItemsById(id);

                if (items == null || items.list.Count == 0)
                {
                    Snack.Title = "Can´t find item";
                    Snack.Content = "The item is not registered in Central Market";
                    Snack.Expand();
                    return;
                }

                items.list.ForEach(item => ItemsControl.Items.Add(item));
            }
            catch (FormatException)
            {
                var items = await BDOMarket.SearchItemsByName(textBox.Text);

                if (items == null || items.list.Count == 0)
                {
                    Snack.Title = "Can´t find item";
                    Snack.Content = "The item is not registered in Central Market";
                    Snack.Expand();
                    return;
                }

                items.list.ForEach(item => ItemsControl.Items.Add(item));
            }

            ItemsControl.Items.Refresh();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (string.IsNullOrEmpty(textBox.Text))
            {
                TextHint.Visibility = Visibility.Visible;
            }
            else
            {
                TextHint.Visibility = Visibility.Collapsed;
            }
        }
    }
}
