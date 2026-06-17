using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomEntryController : ControllerBase
    {
        private readonly ISymptomEntryService _symptomEntryService;
        public SymptomEntryController(ISymptomEntryService symptomEntryService)
        {
            _symptomEntryService = symptomEntryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSymptomEntries([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var entries = await _symptomEntryService.GetAllAsync(from, to);
            return Ok(entries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSymptomEntryById(int id)
        {
            var entry = await _symptomEntryService.GetByIdAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(entry);
        }

        [HttpPost]
        public async Task<IActionResult> AddSymptomEntry(CreateSymptomEntryRequest request)
        {
            var created = await _symptomEntryService.CreateAsync(request);
            return CreatedAtAction(nameof(GetSymptomEntryById),
                new { id = created.Id },
                created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSymptomEntry(int id, UpdateSymptomEntryRequest request)
        {
            var updated = await _symptomEntryService.UpdateAsync(id, request);
            if (updated == null)
            {
                return NotFound();
            }
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSymptomEntry(int id)
        {
            var deleted = await _symptomEntryService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
