using BDO_Spirit.Models;
using Supremes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace BDO_Spirit.Scrapper
{
    public class BDONews
    {

        private const string BaseUrl = "https://www.naeu.playblackdesert.com/en-US/News/Notice?boardType=0";
        private int loadedPages = 0;


        public async Task<List<NewsModel>> LoadNews()
        {
            List<NewsModel> list = new List<NewsModel>();

            var html = await LoadHTML(BaseUrl);

            var doc = Dcsoup.Parse(html);

            var newsList = doc.Select("ul.thumb_nail_list").First.Children;

            foreach (var item in newsList)
            {
                var img = item.Select("img").Attr("src");
                var suffix = item.Select("em").Text;
                var title = item.Select("span.line_clamp").Text;
                var date = item.Select("span.date").Text;
                var url = item.Select("a").Attr("href");

                NewsModel model = new NewsModel(title, suffix, date, img, url);

                list.Add(model);
            }

            loadedPages++;
            return list;
        }
        //&searchType=&searchText=&Page=%page%
        public async Task<List<NewsModel>> LoadOlderNews()
        {
            List<NewsModel> list = new List<NewsModel>();

            var olderPageUrl = BaseUrl + $"&searchType=&searchText=&Page={loadedPages}";

            var html = await LoadHTML(olderPageUrl);

            var doc = Dcsoup.Parse(html);

            var newsList = doc.Select("ul.thumb_nail_list").First.Children;

            foreach (var item in newsList)
            {
                var img = item.Select("img").Attr("src");
                var suffix = item.Select("em").Text;
                var title = item.Select("span.line_clamp").Text;
                var date = item.Select("span.date").Text;
                var url = item.Select("a").Attr("href");

                NewsModel model = new NewsModel(title, suffix, date, img, url);

                list.Add(model);
            }


            loadedPages++;
            return list;
        }

        public static async Task<NewsModel> LoadFullNews(NewsModel model)
        {
            var html = await LoadHTML(model.NewsUrl);

            var doc = Dcsoup.Parse(html);

            var content = doc.Select("div.contents_area").First.Children;

            var imgUrl = content.Select("img").Attr("src");

            var description = "";

            foreach (var item in content)
            {
                description += item.Text + Environment.NewLine;
            }

            return new NewsModel(model, imgUrl, description);
        }

        private static async Task<string> LoadHTML(string url)
        {
            using(var client = new HttpClient())
             {
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 OPR/82.0.4227.58");
            client.DefaultRequestHeaders.Add("accept-language", "en-US");
                var result = await client.GetAsync(url);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
