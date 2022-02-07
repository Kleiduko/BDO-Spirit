using BDO_Spirit.Events;
using BDO_Spirit.Models;
using BDO_Spirit.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFUI.Controls;

namespace BDO_Spirit.UI.Windows
{
    /// <summary>
    /// Interaktionslogik für PriceAlertWindow.xaml
    /// </summary>
    public partial class PriceAlertWindow : Window
    {

        public PriceAlertWindow()
        {
            InitializeComponent();

            var window = Application.Current.MainWindow as MainWindow;

            window.ObservableService.Observables.ForEach(model => ItemsControl.Items.Add(model));

            EventMaster.OnChangeObservable += EventMaster_OnChangeObservable;
            EventMaster.OnRefreshObservables += EventMaster_OnRefreshObservables;
        }

        private void EventMaster_OnRefreshObservables(ObservablesRefreshEventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
                ItemsControl.Items.Clear();
                args.ObservableModels.ForEach(model => ItemsControl.Items.Add(model));
            });
        }

        private void EventMaster_OnChangeObservable(ChangeObservableEventArgs args)
        {
            if (args.ChangeType == ChangeType.Remove)
            {
                ItemsControl.Items.Remove(args.ObservableModel);
            }
            else if(args.ChangeType == ChangeType.Add)
            {
                ItemsControl.Items.Add(args.ObservableModel);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var item = GetItem(sender);

            var textBox = sender as NumberBox;

            item.PriceAlert = Convert.ToInt64(textBox.Text);

            var eventArgs = new ChangeObservableEventArgs()
            {
                ChangeType = ChangeType.Modify,
                ObservableModel = item
            };

            EventMaster.CallOnChangeObservable(eventArgs);
        }

        private ObservableModel GetItem(object sender)
        {
            var window = Application.Current.MainWindow as MainWindow;

            var numberBox = sender as NumberBox;

            var grid = UIHelper.FindVisualParentByName<Grid>(numberBox, "ItemGrid");

            var textBlock = UIHelper.FindVisualChildByName<TextBlock>(grid, "ItemName");

            return window.ObservableService.Observables.Find(x => x.BulkItemSearch.name == textBlock.Text);
        }
    }
}
