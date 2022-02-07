using BDO_Spirit.Api.Models.BDOMarket;
using BDO_Spirit.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BDO_Spirit.Api
{
    public class BDOMarket
    {
        private const string ItemSearchByNameUrl = "https://bdo-api-helper.herokuapp.com/marketplace-clone/item-search/%name%?region=eu";
        private const string ItemSearchByIdUrl = "https://bdo-api-helper.herokuapp.com/api/item-search/%id%?enhLevel=0&region=eu";
        private const string ItemBulkSearchByIdUrl = "https://bdo-api-helper.herokuapp.com/api/search?region=eu";

        public static async Task<MarketNameSearch> SearchItemsByName(string name)
        {
            var validName = name.Replace(" ", "+");
            var url = ItemSearchByNameUrl.Replace("%name%", validName);

            var result = await LoadResult(url);

            var item = JsonConvert.DeserializeObject<MarketNameSearch>(result);

            if (item == null)
            {
                return null;
            }

            return item;
        }

        public static async Task<MarketNameSearch> SearchItemsById(int id)
        {
            var url = ItemSearchByIdUrl.Replace("%id%", id.ToString());

            var result = await LoadResult(url);

            var item = JsonConvert.DeserializeObject<MarketIdSerachItem>(result);

            if (item == null)
            {
                return null;
            }

            var simpleItem = new MarketNameSearchItem()
            {
                mainKey = item.id,
                sumCount = item.totalTrades,
                totalSumCount = item.price,
                name = item.name,
                icon = item.icon,
                grade = item.grade
            };

            var resultItem = new MarketNameSearch();

            resultItem.resultMsg = "";
            resultItem.resultCode = 200;

            resultItem.list = new List<MarketNameSearchItem>();
            resultItem.list.Add(simpleItem);

            return resultItem;
        }

        public static async Task<List<BulkItemSearch>> BulkSearchItemsById(int[] id)
        {
            var result = await LoadResult(ItemBulkSearchByIdUrl, id);

            var item = JsonConvert.DeserializeObject<List<BulkItemSearch>>(result);

            if (item == null)
            {
                return null;
            }

            return item;
        }

        private static async Task<string> LoadResult(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 OPR/82.0.4227.58");
                client.DefaultRequestHeaders.Add("accept-language", "de-DE");
                var result = await client.GetAsync(url);
                return await result.Content.ReadAsStringAsync();
            }
        }

        private static async Task<string> LoadResult(string url, int [] header)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(url);

                    var json = JsonConvert.SerializeObject(new BulkSearchRequest() { ids = header });

                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    var result = await client.SendAsync(request);

                    return await result.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
