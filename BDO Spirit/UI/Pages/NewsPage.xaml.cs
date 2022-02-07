using BDO_Spirit.Models;
using BDO_Spirit.Scrapper;
using BDO_Spirit.UI.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDO_Spirit.UI.Pages
{
    /// <summary>
    /// Interaktionslogik für NewsPage.xaml
    /// </summary>
    public partial class NewsPage : Page
    {
        private BDONews NewsClient;

        public NewsPage()
        {
            EventManager.RegisterClassHandler(typeof(Border), Border.MouseLeftButtonDownEvent, new RoutedEventHandler(this.MouseLeftButtonDownClassHandler));

            Properties.Resources.load_older.Save(AppDomain.CurrentDomain.BaseDirectory + "ItemIcons/load_older.png");

            InitializeComponent();

            LoadNews();     
        }

        private async void LoadNews()
        {
            NewsClient = new BDONews();
            var list = await NewsClient.LoadNews();

            foreach (var item in list)
            {
                ItemsControl.Items.Add(item);
            }

            var newsItem = new NewsModel("Older news from Black Desert", "", "Load older news", AppDomain.CurrentDomain.BaseDirectory + "ItemIcons/load_older.png", "");

            ItemsControl.Items.Add(newsItem);
        }

        private async void MouseLeftButtonDownClassHandler(object sender, RoutedEventArgs e)
        {
            var border = sender as Border;

            if(border.Name != "newsBorder")
            {
                return;
            }

            var grid = border.Child as Grid;

            var textblock = grid.Children[1] as TextBlock;

            foreach(var item in ItemsControl.Items)
            {
                var i = item as NewsModel;

                if(i.Date == "Load older news")
                {
                    var list = await NewsClient.LoadOlderNews();

                   foreach(var item2 in list)
                    {
                        var lastItem = ItemsControl.Items.Count - 1;
                        ItemsControl.Items.Insert(lastItem, item2);
                    }

                    return;
                }

                if (i.Title.Equals(textblock.Text))
                {
                    var fullNews = await BDONews.LoadFullNews(i);

                    new NewsWindow(fullNews).Show();
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var list = await NewsClient.LoadNews();

            ItemsControl.Items.Clear();
            foreach (var item in list)
            {
                ItemsControl.Items.Add(item);
            }

            var newsItem = new NewsModel("Older news from Black Desert", "", "Load older news", AppDomain.CurrentDomain.BaseDirectory + "ItemIcons/load_older.png", "");

            ItemsControl.Items.Add(newsItem);
        }
    }
}
