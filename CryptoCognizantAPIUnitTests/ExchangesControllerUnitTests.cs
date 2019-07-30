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


    }
}

