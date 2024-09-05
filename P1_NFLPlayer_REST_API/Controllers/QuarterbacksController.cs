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
    // This controller handles all CRUD operations for the "Quarterbacks" table
    [Route("api/[controller]")]
    [ApiController]
    public class QuarterbacksController : ControllerBase
    {
        // The database contect to interact with the NFL Player database
        private readonly NflplayerDbContext _context = new NflplayerDbContext();

        //public QuarterbacksController(NflplayerDbContext context)
        //{
        //    _context = context;
        //}

        // GET: api/Quarterbacks
        // Retrieves all quarterback records from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quarterback>>> GetQuarterbacks()
        {
            // 'ActionResult<IEnumerable<Quarterback>>` allows returning a variety of results,
            // including a list of Quarterback entities or error responses.
            // `IEnumerable<Quarterback> is an interface representing a collection of Quarterback objects
            // `async` indicates that this method is asynchronous and uses `await` for non-blocking operations,
            // non-blocking means operations do not prevent the execution of other code while waiting for a task to complete
            return await _context.Quarterbacks.ToListAsync(); // Return the list of quarterbacks
        }

        // GET: api/Quarterbacks/5
        // Retrieves a specific quarterback by their ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Quarterback>> GetQuarterback(int id)
        {// `ActionResults<Quarterback>` allows returning the quarterback object or an error response
            var quarterback = await _context.Quarterbacks.FindAsync(id); // Finds the QB by ID

            if (quarterback == null)
            {
                return NotFound(); // Returns 404 if the QB is not found
            }

            return quarterback; // Returns the QB data
        }

        // PUT: api/Quarterbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Updates an existing quarterback's data
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuarterback(int id, Quarterback quarterback)
        {
            // Checks if the ID in the request matches the QB's ID
            if (id != quarterback.Qbid)
            {
                return BadRequest(); // Returns 400 if the IDs don't match
            }

            // Marks the QB entity as modifies in the DbContext
            _context.Entry(quarterback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); // Saves the changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                // Checks if the QB still exists in the database
                if (!QuarterbackExists(id))
                {
                    return NotFound(); // Returns 404 if the QB is no longer present
                }
                else
                {
                    throw; // Re-throws the exception if another issue occurred
                }
            }

            return NoContent(); // Returns 204 if the update was successful
        }
        // PATCH api/Quarterbacks/{id}
        // Applies partial updates to a quarterbacl's data using JSON Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchQuarterback(int id, [FromBody] JsonPatchDocument<Quarterback> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest(); // Returns 400 if the patch document is null
            }

            var quarterback = await _context.Quarterbacks.FindAsync(id); // Finds the QB by ID
            if (quarterback == null)
            {
                return NotFound(); // returns 404 if the QB is not found
            }

            // Applies the patch document to the QB Entity
            patchDoc.ApplyTo(quarterback);

            // validates the patched QB data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 if validation fails
            }

            try
            {
                await _context.SaveChangesAsync(); // Saves the partial update to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuarterbackExists(id))
                {
                    return NotFound(); // Returns 404 if the QB no longer exists
                }
                else
                {
                    throw; // Rethrows the exception if another issue occurred
                }
            }

            return NoContent(); // Returns 204 if the patch was successful
        }

        // POST: api/Quarterbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Quarterback>> PostQuarterback(Quarterback quarterback)
        {
            _context.Quarterbacks.Add(quarterback); // Adds the QB entity to the DbContext
            await _context.SaveChangesAsync(); // Saves the new QB to the database

            // Returns a 201 Created response with the QB's details and location
            return CreatedAtAction("GetQuarterback", new { id = quarterback.Qbid }, quarterback);
        }

        // DELETE: api/Quarterbacks/{id}
        // Deletes a quarterback from the database by their ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuarterback(int id)
        {
            var quarterback = await _context.Quarterbacks.FindAsync(id); // Finds the quarterback by ID
            if (quarterback == null)
            {
                return NotFound(); // returns 404 if the QB is not found
            }

            _context.Quarterbacks.Remove(quarterback); // Removes the QB from the DbContext
            await _context.SaveChangesAsync(); // Saves the changes to the database

            return NoContent(); // Returns 204 after successful deletion
        }

        // Helper method to check if a quarterback exists by their ID
        private bool QuarterbackExists(int id)
        {
            return _context.Quarterbacks.Any(e => e.Qbid == id); // Checks if any QB matches the given ID 
        }
    }
}
