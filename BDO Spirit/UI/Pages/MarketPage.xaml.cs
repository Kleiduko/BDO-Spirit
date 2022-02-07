using BDO_Spirit.Api;
using BDO_Spirit.Api.Models.BDOMarket;
using BDO_Spirit.Events;
using BDO_Spirit.Models;
using BDO_Spirit.Services;
using BDO_Spirit.UI.Windows;
using BDO_Spirit.Util;
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

        private ItemsObservableService ObservableService;

        public MarketPage()
        {
            InitializeComponent();

            var window = Application.Current.MainWindow as MainWindow;

            ObservableService = window.ObservableService;

            EventManager.RegisterClassHandler(typeof(Grid), Grid.MouseLeftButtonDownEvent, new RoutedEventHandler(this.ItemsControlClick));
            EventManager.RegisterClassHandler(typeof(WPFUI.Controls.Icon), WPFUI.Controls.Icon.MouseLeftButtonDownEvent, new RoutedEventHandler(this.HandleObservables));
        }

        private void HandleObservables(object sender, RoutedEventArgs e)
        {
            var icon = sender as WPFUI.Controls.Icon;

            var item = GetClickedItemByParent(icon);

            if (item == null)
            {
                return;
            }
            
            var observableItem = ObservableService.Observables.Find(c => c.Id == item.mainKey);

            if (observableItem != null)
            {
                item.star = WPFUI.Common.Icon.StarAdd24;

                var eventArgs = new ChangeObservableEventArgs();
                eventArgs.ObservableModel = observableItem;
                eventArgs.ChangeType = ChangeType.Remove;
                EventMaster.CallOnChangeObservable(eventArgs);
            }
            else
            {
                if (ObservableService.Observables.Count >= 10)
                {
                    Snack.Title = "Observable items limit reached";
                    Snack.Content = "Observable items has a limit of 10!";
                    Snack.Expand();
                    return;
                }

                var model = new ObservableModel() { Id = item.mainKey, PriceAlert = item.totalSumCount };
                item.star = WPFUI.Common.Icon.Star24;

                var eventArgs = new ChangeObservableEventArgs();
                eventArgs.ObservableModel = model;
                eventArgs.ChangeType = ChangeType.Add;
                EventMaster.CallOnChangeObservable(eventArgs);
            }

            ItemsControl.Items.Refresh();
        }

        private void ItemsControlClick(object sender, RoutedEventArgs e)
        {
            var item = GetClickedItemByChildren(sender as Grid);

            if (item == null)
            {
                return;
            }
            Debug.WriteLine("market");
            //Opens market window
        }

        private MarketNameSearchItem GetClickedItemByChildren(Grid grid)
        {
            if(grid == null || grid.Name != "ItemGrid")
            {
                return null;
            }

            var textBlock = UIHelper.FindVisualChildByName<TextBlock>(grid, "MarketItemName");

            if(textBlock == null)
            {
                return null;
            }

            foreach (var item in ItemsControl.Items)
            {
                var marketItem = item as MarketNameSearchItem;

                if (marketItem.name != textBlock.Text)
                {
                    continue;
                }

                return marketItem;
            }
            return null;
        }

        private MarketNameSearchItem GetClickedItemByParent(WPFUI.Controls.Icon icon)
        {

            var grid = UIHelper.FindVisualParentByName<Grid>(icon, "root");

            if (grid == null)
            {
                return null;
            }

            return GetClickedItemByChildren(grid.Children[0] as Grid);
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
