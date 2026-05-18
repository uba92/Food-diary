using FoodDiary.Api.Data;
using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Helpers;
using FoodDiary.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodAlternativesController : ControllerBase
    {
        private readonly IFoodAlternativeService _service;
        public FoodAlternativesController(IFoodAlternativeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetFoodAlternatives()
        {
            var foodAlternatives = await _service.GetAllAsync();
            return Ok(foodAlternatives);
        }

        [HttpPost]
        public async Task<IActionResult> AddFoodAlternatives(CreateFoodAlternativeRequest request)
        {
            var createdFoodAlternative = await _service.CreateAsync(request);
            return Ok(createdFoodAlternative);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodAlternativeById(int id)
        {
            var foodAlternative = await _service.GetByIdAsync(id);
            if (foodAlternative == null)
            {
                return NotFound();
            }
            return Ok(foodAlternative);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodAlternative(int id)
        {
            bool result = await _service.DeleteAsync(id);
            if(result == false)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFoodAlternative(int id, UpdateFoodAlternativeRequest request)
        {
            var foodAlternative = await _service.UpdateAsync(id, request);
            if (foodAlternative == null)
            {
                return NotFound();
            }
            return Ok(foodAlternative);
        }
    }
}
