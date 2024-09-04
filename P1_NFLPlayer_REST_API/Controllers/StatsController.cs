using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using P1_NFLPlayer_REST_API.POCO;
using Microsoft.AspNetCore.JsonPatch;

namespace P1_NFLPlayer_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly NflplayerDbContext _context = new NflplayerDbContext();

        //public StatsController(NflplayerDbContext context)
        //{
        //    _context = context;
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stat>>> GetStats()
        {
            return await _context.Stats.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PutStat(int id, Stat stat)
        {
            if (id != stat.StatId)
            {
                return BadRequest();
            }

            _context.Entry(stat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) 
            {
                if (!StatExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Stat>> PostStat(Stat stat)
        {
            _context.Stats.Add(stat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStat", new { id = stat.StatId }, stat);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchStat(int id, [FromBody] JsonPatchDocument<Stat> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var stat = await _context.Stats.FindAsync(id);
            if (stat == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(stat);

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!StatExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStat(int id)
        {
            var stat = await _context.Stats.FindAsync(id);
            if (stat == null)
            {
                return NotFound();
            }

            _context.Stats.Remove(stat);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool StatExists(int id)
        {
            return _context.Stats.Any(e => e.StatId == id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutContract(int id, Contract contract)
        {
            if (id != contract.ContractId)
            {
                return BadRequest();
            }

            _context.Entry(contract).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (StatExists(id))
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
    }

    
}
