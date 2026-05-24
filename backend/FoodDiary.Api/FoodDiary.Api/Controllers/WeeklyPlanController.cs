using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyPlanController : ControllerBase
    {
        private readonly IWeeklyPlanService _weeklyPlanService;
        public WeeklyPlanController(IWeeklyPlanService weeklyPlanService)
        {
            _weeklyPlanService = weeklyPlanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeeklyPlan()
        {
            var weeklyPlans = await _weeklyPlanService.GetAllWeeklyPlansAsync();
            return Ok(weeklyPlans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeeklyPlanById(int id)
        {
            var weeklyPlan = await _weeklyPlanService.GetWeeklyPlanByIdAsync(id);
            if(weeklyPlan == null)
            {
                return NotFound();
            }
            return Ok(weeklyPlan);
        }

        [HttpPost]
        public async Task<IActionResult> AddWeeklyPlan(CreateWeeklyPlanRequest request)
        {
            var createdWeeklyPlan = await _weeklyPlanService.CreateWeeklyPlanAsync(request);

            return CreatedAtAction(nameof(GetWeeklyPlanById), 
                new { id = createdWeeklyPlan.Id}, 
                createdWeeklyPlan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeeklyPlan(int id, UpdateWeeklyPlanRequest request)
        {
            var updatedWeeklyPlan = await _weeklyPlanService.UpdateWeeklyPlanAsync(id, request);
            if (!updatedWeeklyPlan)
            {
                return NotFound();
            }
            return Ok(updatedWeeklyPlan);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeeklyPlan(int id)
        {
            var deleted = await _weeklyPlanService.DeleteWeeklyPlanAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
