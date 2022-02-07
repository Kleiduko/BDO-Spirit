using BDO_Spirit.Api;
using BDO_Spirit.Events;
using BDO_Spirit.Models;
using BDO_Spirit.Services;
using Microsoft.Toolkit.Uwp.Notifications;
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
        public ItemsObservableService ObservableService;

        public MainWindow()
        {
            WPFUI.Background.Manager.Apply(this);

            InitializeComponent();

            ObservableService = new ItemsObservableService();

            ObservableService.StartService();
        }

        private void NavigationFluent_Loaded(object sender, RoutedEventArgs e)
        {
            var nav = sender as NavigationFluent;
            nav.Navigate("news");
        }

        private void NavigationFluent_Navigated(object sender, RoutedEventArgs e)
        {

        }

        private void RootTitleBar_CloseClicked(object sender, RoutedEventArgs e)
        {
            ObservableService.StopService();
        }
    }
}
