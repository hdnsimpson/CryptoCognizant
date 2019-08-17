using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoCognizant.Model;
using System.Web.Http.Cors;

namespace CryptoCognizant.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    [Route("api/[controller]")]
    [ApiController]
    public class ExchangesController : ControllerBase
    {
        private readonly CryptoCognizantContext _context;

        public ExchangesController(CryptoCognizantContext context)
        {
            _context = context;
        }

        // GET: api/Exchanges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exchange>>> GetExchange()
        {
            return await _context.Exchange.ToListAsync();
        }

        // GET: api/Exchanges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exchange>> GetExchange(int id)
        {
            var exchange = await _context.Exchange.FindAsync(id);

            if (exchange == null)
            {
                return NotFound();
            }

            return exchange;
        }

        // PUT: api/Exchanges/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExchange(int id, Exchange exchange)
        {
            if (id != exchange.ExchangeId)
            {
                return BadRequest();
            }

            _context.Entry(exchange).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExchangeExists(id))
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

        // POST: api/Exchanges
        [HttpPost]
        public async Task<ActionResult<Exchange>> PostExchange(Exchange exchange)
        {
            _context.Exchange.Add(exchange);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExchange", new { id = exchange.ExchangeId }, exchange);
        }

        // DELETE: api/Exchanges/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Exchange>> DeleteExchange(int id)
        {
            var exchange = await _context.Exchange.FindAsync(id);
            if (exchange == null)
            {
                return NotFound();
            }

            _context.Exchange.Remove(exchange);
            await _context.SaveChangesAsync();

            return exchange;
        }

        private bool ExchangeExists(int id)
        {
            return _context.Exchange.Any(e => e.ExchangeId == id);
        }
    }
}
