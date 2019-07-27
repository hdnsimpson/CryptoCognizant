using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace CryptoCognizant.Model
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<Coin, CoinDTO>();
            CreateMap<CoinDTO, Coin>();
        }
    }
}
