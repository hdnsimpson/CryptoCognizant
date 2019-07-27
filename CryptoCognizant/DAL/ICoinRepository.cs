using System;
using System.Collections.Generic;
using CryptoCognizant.Model;

namespace CryptoCognizant.DAL
{
    public interface ICoinRepository : IDisposable
    {
        IEnumerable<Coin> GetCoins();
        Coin GetCoinByID(int CoinId);
        void InsertCoin(Coin coin);
        void DeleteCoin(int CoinId);
        void UpdateCoin(Coin coin);
        void Save();
    }
}