using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CryptoCognizant.Model;
using Newtonsoft.Json;

namespace CryptoCognizant.Helper
{
    public class CryptoCompareHelper
    {
        public static void testProgram()
        {
            getCoinInfo("BTC");
        }

        public static Coin getCoinInfo(string coinSymbol)
        {
            String APIKey = "a0c8a2b11026c8b60df5fefec1c5891f5924c602226d578c9e4afb601966efb3";
            String CryptoCompareAPIURL = "https://min-api.cryptocompare.com/data/pricemultifull?fsyms=" + coinSymbol + "&tsyms=USD&api_key=" + APIKey;

            // Use an http client to grab the JSON string from the web.
            String coinInfoJSON = new WebClient().DownloadString(CryptoCompareAPIURL);

            // Using dynamic object helps us to more efficiently extract information from a large JSON String.
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(coinInfoJSON);

            // Extract information from the dynamic object.
            String coinSymbol1 = jsonObj["RAW"][coinSymbol]["USD"]["FROMSYMBOL"];
            String imageUrl = jsonObj["RAW"][coinSymbol]["USD"]["IMAGEURL"];

            Coin coin = new Coin
            {
                CoinSymbol = coinSymbol1,
                ImageUrl = imageUrl
            };

            return coin;
        }

        public static List<Exchange> getExchanges(string coinSymbol)
        {
            String APIKey = "a0c8a2b11026c8b60df5fefec1c5891f5924c602226d578c9e4afb601966efb3";
            String CryptoCompareAPIURL = "https://min-api.cryptocompare.com/data/v3/all/exchanges?fsym=" + coinSymbol + "&api_key=" + APIKey;

            // Use an http client to grab the JSON string from the web.
            String exchangeInfoJSON = new WebClient().DownloadString(CryptoCompareAPIURL);

            // Using dynamic object helps us to more efficiently extract information from a large JSON String.
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(exchangeInfoJSON);

            List<Exchange> exchanges = new List<Exchange>();

            // Extract information from the dynamic object.
            String data = jsonObj["Data"].ToString(Formatting.None);

            string[] names = data.Split(new string[] { "\"" }, StringSplitOptions.None);
            int i = 0;
            foreach (string name in names)
            {
                if (name == "pairs")
                {
                    String exchangeName = names[i-2];
                    String isActive = jsonObj["Data"][exchangeName]["isActive"];
                    Boolean isActiveBool = false;
                    if (isActive == "True")
                    {
                        isActiveBool = true;
                    }
                    String pairs = jsonObj["Data"][exchangeName]["pairs"][coinSymbol].ToString(Formatting.None);
                    pairs = pairs.Replace("\"", "");
                    pairs = pairs.Replace("[", "");
                    pairs = pairs.Replace("]", "");

                    Exchange exchange = new Exchange
                    {
                        ExchangeName = exchangeName,
                        IsActive = isActiveBool,
                        Pairs = pairs,
                        CoinSymbol = coinSymbol
                    };

                    exchanges.Add(exchange);
                }

                i++;
            }   

            return exchanges;
        }
    }
}
