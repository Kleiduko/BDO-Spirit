using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using WPFUI.Common;

namespace BDO_Spirit.Api.Models.BDOMarket
{
    public class MarketNameSearchItem
    {
        public int mainKey { get; set; }
        public long sumCount { get; set; }
        public long totalSumCount { get; set; }
        public string name { get; set; }
        public int grade { get; set; }
        public string icon { get; set; }

        private Icon? observableIcon;

        public string formattedPrice
        {
            get
            {
                return totalSumCount.ToString("N0");
            }
        }

        public SolidColorBrush gradeColor
        {
            get
            {
                switch (grade)
                {
                    case 0: return new SolidColorBrush(Color.FromRgb(64, 64, 66));
                    case 1: return new SolidColorBrush(Color.FromRgb(135, 163, 83));
                    case 2: return new SolidColorBrush(Color.FromRgb(82, 142, 194));
                    case 3: return new SolidColorBrush(Color.FromRgb(210, 163, 61));
                    case 4: return new SolidColorBrush(Color.FromRgb(181, 112, 101));
                }
                return new SolidColorBrush(Colors.White);
            }
        }

        public Icon star
        {
            get
            {
                if (observableIcon == null)
                {
                    LoadIcon();
                }
                return (Icon)observableIcon;
            }

            set
            {
                observableIcon = value;
            }
        }

        private void LoadIcon()
        {
            var window = Application.Current.MainWindow as MainWindow;

            var items = window.ObservableService.Observables;

            if (items == null || items.Count == 0)
            {
                observableIcon = Icon.StarAdd24;
                return;
            }

            foreach (var item in items)
            {
                if (item.Id == mainKey)
                {
                    observableIcon = Icon.Star24;
                    return;
                }
            }

            if (observableIcon == null)
            {
                observableIcon = Icon.StarAdd24;
            }
        }
    }
}