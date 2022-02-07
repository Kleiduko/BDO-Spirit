using BDO_Spirit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BDO_Spirit.Models
{
    public class BulkItemSearch
    {

        public string name { get; set; }
        public int id { get; set; }
        public long price { get; set; }
        public long count { get; set; }
        public int enhanceGrade { get; set; }
        public long totalTrades { get; set; }
        public string icon { get; set; }
        public int grade { get; set; }

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

        public string formattedPrice
        {
            get
            {
                return price.ToString("N0");
            }
        }

        public string image
        {
            get
            {
                string path = ItemIconService.GetIconPath(name);
                if (path != null)
                {
                    return path;
                }
                return icon;
            }
        }
    }
}
