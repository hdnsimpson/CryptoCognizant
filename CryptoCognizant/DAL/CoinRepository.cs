using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CryptoCognizant.Model;

namespace CryptoCognizant.DAL
{
    public class CoinRepository : ICoinRepository, IDisposable
    {
        private CryptoCognizantContext context;

        public CoinRepository(CryptoCognizantContext context)
        {
            this.context = context;
        }

        public IEnumerable<Coin> GetCoins()
        {
            return context.Coin.ToList();
        }

        public Coin GetCoinByID(int id)
        {
            return context.Coin.Find(id);
        }

        public void InsertCoin(Coin coin)
        {
            context.Coin.Add(coin);
        }

        public void DeleteCoin(int coinId)
        {
            Coin coin = context.Coin.Find(coinId);
            context.Coin.Remove(coin);
        }

        public void UpdateCoin(Coin coin)
        {
            context.Entry(coin).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}