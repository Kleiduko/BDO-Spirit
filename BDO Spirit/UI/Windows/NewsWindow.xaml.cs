using BDO_Spirit.Models;
using System;
using System.Collections.Generic;
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

namespace BDO_Spirit.UI.Windows
{
    /// <summary>
    /// Interaktionslogik für NewsWindow.xaml
    /// </summary>
    public partial class NewsWindow : Window
    {
        public NewsModel NewsModel { get; private set; } 

        public NewsWindow(NewsModel model)
        {
            NewsModel = model;  

            InitializeComponent();

            Loaded += NewsWindow_Loaded;
        }

        public void NewsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TitleBar.Title = NewsModel.Suffix + " " + NewsModel.Title + " - " + NewsModel.Date;
            Thumbnail.ImageSource = new BitmapImage(new Uri(NewsModel.ImageUrl));

            Description.Text = NewsModel.Description;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(NewsModel.NewsUrl);
        }
    }
}
