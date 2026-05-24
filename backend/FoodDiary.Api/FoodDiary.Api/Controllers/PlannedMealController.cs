using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlannedMealController : ControllerBase
    {
        private readonly IPlannedMealService _plannedMealService;
        public PlannedMealController(IPlannedMealService plannedMealService)
        {
            _plannedMealService = plannedMealService;   
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlannedMeals()
        {
            var meals = await _plannedMealService.GetAllPlannedMealsAsync();
            return Ok(meals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlannedMeal(int id)
        {
            var meal = await _plannedMealService.GetPlannedMealByIdAsync(id);
            if(meal == null)
            {
                return NotFound();
            }
            return Ok(meal);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlannedMeal(CreatePlannedMealRequest request)
        {
            var meal = await _plannedMealService.CreatePlannedMealAsync(request);
            if(meal == null)
            {
                return BadRequest("Invalid WeeklyPlanId or FoodAlternativeId.");
            }
            return CreatedAtAction(nameof(GetPlannedMeal), 
                new {id = meal.Id},
                meal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlannedMeal(int id, UpdatePlannedMealRequest request)
        {
            var update = await _plannedMealService.UpdatePlannedMealAsync(id, request);
            if(!update)
            {
                return NotFound();
            }
            return Ok(update);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlannedMeal(int id)
        {
            var delete = await _plannedMealService.DeletePlannedMealAsync(id);
            if(!delete)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
