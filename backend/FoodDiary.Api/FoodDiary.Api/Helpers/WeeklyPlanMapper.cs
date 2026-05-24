using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Helpers
{
    public class WeeklyPlanMapper
    {
        public static WeeklyPlan ToEntity(CreateWeeklyPlanRequest request)
        {
            return new WeeklyPlan
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            };
        }

        public static WeeklyPlanResponseDto ToResponseDto(WeeklyPlan weeklyPlan)
        {
            return new WeeklyPlanResponseDto
            {
                Id = weeklyPlan.Id,
                StartDate = weeklyPlan.StartDate,
                EndDate = weeklyPlan.EndDate,
                PlannedMeals = weeklyPlan.PlannedMeals.Select(pm => new PlannedMealResponseDto
                {
                    Id = pm.Id,
                    DayOfWeek = pm.DayOfWeek,
                    FoodAlternativeId = pm.FoodAlternativeId,
                    WeeklyPlanId = pm.WeeklyPlanId
                }).ToList()
            };
        }
    }
}
