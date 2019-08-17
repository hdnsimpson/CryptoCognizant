using System;
using System.Collections.Generic;
using System.Text;
using CryptoCognizant.Controllers;
using CryptoCognizant.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CryptoCognizantAPIUnitTests
{
    [TestClass]
    public class CoinsControllerUnitTests
    {
        public static readonly DbContextOptions<CryptoCognizantContext> options
            = new DbContextOptionsBuilder<CryptoCognizantContext>()
            .UseInMemoryDatabase(databaseName: "testDatabase").Options;

        public static readonly IList<Coin> coins = new List<Coin>
        {
            new Coin()
            {
                CoinId = 1,
                CoinSymbol = "BTC",
                IsFavourite = true
            },
            new Coin()
            {
                CoinId = 2,
                CoinSymbol = "NAV",
                IsFavourite = false
            }
        };

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                // populate the db
                context.Coin.Add(coins[0]);
                context.Coin.Add(coins[1]);
                context.SaveChanges();
            }
        }

        [TestCleanup]
        public void ClearDb()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                // clear the db
                context.Coin.RemoveRange(context.Coin);
                context.SaveChanges();
            };
        }

        // Make sure results are returned when GetCoin() is called
        [TestMethod]
        public async Task TestGetSuccessfully()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                CoinsController coinsController = new CoinsController(context);
                ActionResult<IEnumerable<Coin>> result = await coinsController.GetCoin();

                Assert.IsNotNull(result);
            }
        }
        
        // Make sure the correct coin is returned when GetCoin(id) is called
        [TestMethod]
        public async Task TestGetIDSuccessfully()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                CoinsController coinsController = new CoinsController(context);
                ActionResult<Coin> result = await coinsController.GetCoin(0);

                Assert.IsNotNull(result);
            }
        }
        
        // Test getting a no content status code when PutCoin() is called
        [TestMethod]
        public async Task TestPutCoinNoContentStatusCode()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                Boolean newFav = false;
                Coin coin1 = context.Coin .Where(x => x.IsFavourite == coins[0].IsFavourite).Single();
                coin1.IsFavourite = newFav;

                CoinsController coinsController = new CoinsController(context);
                IActionResult result = await coinsController.PutCoin(coin1.CoinId, coin1) as IActionResult;

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }
        }
        /*
        // Test post method DeleteExchange()
        [TestMethod]
        public async Task TestDeleteSuccessfully()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                ExchangesController exchangesController = new ExchangesController(context);

                ActionResult<IEnumerable<Exchange>> result1 = await exchangesController.GetExchange();

                ActionResult<Exchange> delete = await exchangesController.DeleteExchange(0);

                ActionResult<IEnumerable<Exchange>> result2 = await exchangesController.GetExchange();

                // Assert that exchange list changes after exchange is deleted
                Assert.AreNotEqual(result1, result2);
            }
        }

        // Test post method PostExchange()
        [TestMethod]
        public async Task TestPostSuccessfully()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                ExchangesController exchangesController = new ExchangesController(context);

                Exchange exch = new Exchange()
                {
                    ExchangeId = 3,
                    Pairs = "USD, USDT, NZD, AUD"
                };

                ActionResult<IEnumerable<Exchange>> result1 = await exchangesController.GetExchange();

                ActionResult<Exchange> post = await exchangesController.PostExchange(exch);

                ActionResult<IEnumerable<Exchange>> result2 = await exchangesController.GetExchange();

                // Assert that exchange list changes after new exchange is posted
                Assert.AreNotEqual(result1, result2);
            }
        }*/

    }
}

