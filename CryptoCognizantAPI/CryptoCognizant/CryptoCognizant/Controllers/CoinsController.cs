using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoCognizant.Model;
using CryptoCognizant.Helper;

namespace CryptoCognizant.Controllers
{
    public class SYMDTO
    {
        public string SYM { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly CryptoCognizantContext _context;

        public CoinsController(CryptoCognizantContext context)
        {
            _context = context;
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

        private bool CoinExists(int id)
        {
            return _context.Coin.Any(e => e.CoinId == id);
        }
    }
}
