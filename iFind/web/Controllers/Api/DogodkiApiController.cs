using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers_Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogodkiApiController : ControllerBase
    {
        private readonly iFindContext _context;

        public DogodkiApiController(iFindContext context)
        {
            _context = context;
        }

        // GET: api/DogodkiApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dogodek>>> GetDogodek()
        {
            return await _context.Dogodek.ToListAsync();
        }

        // GET: api/DogodkiApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dogodek>> GetDogodek(int id)
        {
            var dogodek = await _context.Dogodek.FindAsync(id);

            if (dogodek == null)
            {
                return NotFound();
            }

            return dogodek;
        }

        // PUT: api/DogodkiApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDogodek(int id, Dogodek dogodek)
        {
            if (id != dogodek.Id)
            {
                return BadRequest();
            }

            _context.Entry(dogodek).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DogodekExists(id))
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

        // POST: api/DogodkiApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dogodek>> PostDogodek(Dogodek dogodek)
        {
            _context.Dogodek.Add(dogodek);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDogodek", new { id = dogodek.Id }, dogodek);
        }

        // DELETE: api/DogodkiApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDogodek(int id)
        {
            var dogodek = await _context.Dogodek.FindAsync(id);
            if (dogodek == null)
            {
                return NotFound();
            }

            _context.Dogodek.Remove(dogodek);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DogodekExists(int id)
        {
            return _context.Dogodek.Any(e => e.Id == id);
        }
    }
}
