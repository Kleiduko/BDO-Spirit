using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BDO_Spirit.Models
{
    public class NewsModel
    {

        public string Title { get; set; }
        public string Suffix { get; set; }
        public string Date { get; set; }
        public string ImageUrl { get; set; }

        public string NewsUrl { get; set; }

        public string Description { get; set; }

        public NewsModel(string title, string suffix, string date, string imageUrl, string newsUrl)
        {
            Title = title;
            Suffix = suffix;
            Date = date;
            ImageUrl = imageUrl;
            NewsUrl = newsUrl;
        }

        public NewsModel(NewsModel model, string imgUrl, string description) : this(model.Title, model.Suffix, model.Date, imgUrl, model.NewsUrl)
        {
            Description = description;
        }
    }
}
