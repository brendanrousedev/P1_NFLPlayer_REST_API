using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P1_NFLPlayer_REST_API.POCO;

namespace P1_NFLPlayer_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuarterbacksController : ControllerBase
    {
        private readonly NflplayerDbContext _context = new NflplayerDbContext();

        //public QuarterbacksController(NflplayerDbContext context)
        //{
        //    _context = context;
        //}

        // GET: api/Quarterbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quarterback>>> GetQuarterbacks()
        {
            return await _context.Quarterbacks.ToListAsync();
        }

        // GET: api/Quarterbacks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quarterback>> GetQuarterback(int id)
        {
            var quarterback = await _context.Quarterbacks.FindAsync(id);

            if (quarterback == null)
            {
                return NotFound();
            }

            return quarterback;
        }

        // PUT: api/Quarterbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuarterback(int id, Quarterback quarterback)
        {
            if (id != quarterback.Qbid)
            {
                return BadRequest();
            }

            _context.Entry(quarterback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuarterbackExists(id))
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchQuarterback(int id, [FromBody] JsonPatchDocument<Quarterback> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var quarterback = await _context.Quarterbacks.FindAsync(id);
            if (quarterback == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(quarterback);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuarterbackExists(id))
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

        // POST: api/Quarterbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Quarterback>> PostQuarterback(Quarterback quarterback)
        {
            _context.Quarterbacks.Add(quarterback);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuarterback", new { id = quarterback.Qbid }, quarterback);
        }

        // DELETE: api/Quarterbacks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuarterback(int id)
        {
            var quarterback = await _context.Quarterbacks.FindAsync(id);
            if (quarterback == null)
            {
                return NotFound();
            }

            _context.Quarterbacks.Remove(quarterback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuarterbackExists(int id)
        {
            return _context.Quarterbacks.Any(e => e.Qbid == id);
        }
    }
}
