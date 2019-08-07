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
using System.Web.Mvc;

namespace CryptoCognizantAPIUnitTests
{
    [TestClass]
    class ExchangesControllerUnitTests
    {
        public static readonly DbContextOptions<CryptoCognizantContext> options 
            = new DbContextOptionsBuilder<CryptoCognizantContext>()
            .UseInMemoryDatabase(databaseName: "testDatabase").Options;

        public static readonly IList<Exchange> exchanges = new List<Exchange>
        {
            new Exchange()
            {
                Pairs = "BTC, ETH, NAV"
            },
            new Exchange()
            {
                Pairs = "USD, USDT"
            }
        };

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                // populate the db
                context.Exchange.Add(exchanges[0]);
                context.Exchange.Add(exchanges[1]);
                context.SaveChanges();
            }
        }

        [TestCleanup]
        public void ClearDb()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                // clear the db
                context.Exchange.RemoveRange(context.Exchange);
                context.SaveChanges();
            };
        }

        // Make sure results are returned when GetExchange() is called
        [TestMethod]
        public async Task TestGetSuccessfully()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                ExchangesController exchangesController = new ExchangesController(context);
                ActionResult<IEnumerable<Exchange>> result = await exchangesController.GetExchange();

                Assert.IsNotNull(result);
            }
        }

        // Make sure the correct exchange is returned when GetExchange(id) is called
        [TestMethod]
        public async Task TestGetIDSuccessfully()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                ExchangesController exchangesController = new ExchangesController(context);
                ActionResult<Exchange> result = await exchangesController.GetExchange(0);

                Assert.IsNotNull(result);
                Assert.AreEqual(result, exchanges[0]);
            }
        }

        // Test getting a no content status code when PutExchange() is called
        [TestMethod]
        public async Task TestPutExchangeNoContentStatusCode()
        {
            using (var context = new CryptoCognizantContext(options))
            {
                string newPairs = "BTC, NAV, LTC";
                Exchange exchange1 = context.Exchange.Where(x => x.Pairs == exchanges[0].Pairs).Single();
                exchange1.Pairs = newPairs;

                ExchangesController exchangesController = new ExchangesController(context);
                IActionResult result = await exchangesController.PutExchange(exchange1.ExchangeId, exchange1) as IActionResult;

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }
        }


    }
}

