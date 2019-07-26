using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    }
}
