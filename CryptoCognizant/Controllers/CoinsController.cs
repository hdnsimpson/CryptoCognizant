using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoCognizant.Model;
using CryptoCognizant.Helper;
using Microsoft.AspNetCore.Mvc;
using CryptoCognizant.DAL;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using System.Web.Http.Cors;

namespace CryptoCognizant.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]

    public class SYMDTO
    {
        public string SYM { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly CryptoCognizantContext _context;
        private ICoinRepository coinRepository;
        private readonly IMapper _mapper;

        public CoinsController(CryptoCognizantContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            this.coinRepository = new CoinRepository(new CryptoCognizantContext());
        }

        // GET: api/Coins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coin>>> GetCoin()
        {
            return await _context.Coin.ToListAsync();
        }

        // GET: api/Coins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coin>> GetCoin(int id)
        {
            var coin = await _context.Coin.FindAsync(id);

            if (coin == null)
            {
                return NotFound();
            }

            return coin;
        }

        // PUT: api/Coins/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoin(int id, Coin coin)
        {
            if (id != coin.CoinId)
            {
                return BadRequest();
            }

            _context.Entry(coin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoinExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //PUT with PATCH to handle isFavourite
        [HttpPatch("update/{id}")]
        public CoinDTO Patch(int id, [FromBody]JsonPatchDocument<CoinDTO> coinPatch)
        {
            //get original coin object from the database
            Coin originCoin = coinRepository.GetCoinByID(id);
            //use automapper to map that to DTO object
            CoinDTO coinDTO = _mapper.Map<CoinDTO>(originCoin);
            //apply the patch to that DTO
            coinPatch.ApplyTo(coinDTO);
            //use automapper to map the DTO back ontop of the database object
            _mapper.Map(coinDTO, originCoin);
            //update coin in the database
            _context.Update(originCoin);
            _context.SaveChanges();
            return coinDTO;
        }

        // POST: api/Coins
        [HttpPost]
        public async Task<ActionResult<Coin>> PostCoin([FromBody]SYMDTO data)
        {
            Coin coin;
            String coinSymbol;
            try
            {
                // Constructing the video object from our helper function
                coinSymbol = data.SYM;
                coin = CryptoCompareHelper.getCoinInfo(coinSymbol);
            }
            catch
            {
                return BadRequest("Invalid Coin Symbol");
            }

            // Add this video object to the database
            _context.Coin.Add(coin);
            await _context.SaveChangesAsync();

            // Get the primary key
            int id = coin.CoinId;

            // Insert exchanges in a non-blocking fashion
            CryptoCognizantContext tempContext = new CryptoCognizantContext();
            ExchangesController exchangesController = new ExchangesController(tempContext);

            // Add exchanges on another thread
            Task addExchanges = Task.Run(async () =>
            {
                // Get a list of exchanges from CryptoCompareHelper
                List<Exchange> exchanges = new List<Exchange>();
                exchanges = CryptoCompareHelper.getExchanges(coinSymbol);

                for (int i = 0; i < exchanges.Count; i++)
                {
                    // Get the exchange objects from exchanges and assign coinSymbol to CoinSymbol, the primary key of the newly inserted coin
                    Exchange exchange = exchanges.ElementAt(i);
                    exchange.CoinSymbol = coinSymbol;
                    // Add this exchange to the database
                    await exchangesController.PostExchange(exchange);
                }
            });

            // Return success code and the info on the video object
            return CreatedAtAction("GetCoin", new { id = coin.CoinId }, coin);
        }

        // DELETE: api/Coins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Coin>> DeleteCoin(int id)
        {
            var coin = await _context.Coin.FindAsync(id);
            if (coin == null)
            {
                return NotFound();
            }

            _context.Coin.Remove(coin);
            await _context.SaveChangesAsync();

            return coin;
        }

        // GET api/Coins/SearchByPairs/5
        [HttpGet("SearchByPairs/{searchString}")]
        public async Task<ActionResult<IEnumerable<Coin>>> Search(string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                return BadRequest("Search string cannot be null or empty.");
            }

            var coins = await _context.Coin.Include(coin => coin.Exchange).Select(coin => new Coin
            {
                CoinId = coin.CoinId,
                CoinSymbol = coin.CoinSymbol,
                ImageUrl = coin.ImageUrl,
                IsFavourite = coin.IsFavourite,
                Exchange = coin.Exchange.Where(exch => exch.Pairs.Contains(searchString)).ToList()
            }).ToListAsync();

            // Removes all coins with empty trading pairs
            coins.RemoveAll(coin => coin.Exchange.Count == 0);

            return Ok(coins);
        }

        private bool CoinExists(int id)
        {
            return _context.Coin.Any(e => e.CoinId == id);
        }
    }
}
