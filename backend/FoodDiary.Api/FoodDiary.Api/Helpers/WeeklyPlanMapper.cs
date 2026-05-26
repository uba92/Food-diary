using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Enums;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Helpers
{
    public class WeeklyPlanMapper
    {
        public static WeeklyPlan ToEntity(CreateWeeklyPlanRequest request)
        {
            return new WeeklyPlan
            {
                StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc)
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
                    WeeklyPlanId = pm.WeeklyPlanId,
                    FoodAlternativeName = pm.FoodAlternative != null ? pm.FoodAlternative.Name : string.Empty,
                    MealType = pm.FoodAlternative != null ? pm.FoodAlternative.MealType : string.Empty,
                    Quantity = pm.FoodAlternative != null ? pm.FoodAlternative.Quantity : string.Empty
                }).ToList()
            };
        }
        public static WeeklyPlanByDayResponseDto ToByDayResponseDto(WeeklyPlan weeklyPlan)
        {
            return new WeeklyPlanByDayResponseDto
            {
                Id = weeklyPlan.Id,
                StartDate = weeklyPlan.StartDate,
                EndDate = weeklyPlan.EndDate,
                Monday = weeklyPlan.PlannedMeals.Where(pm => pm.DayOfWeek == DayOfWeekType.Monday)
                .Select(pm => PlannedMealMapper.ToResponseDto(pm)).ToList(),
                Tuesday = weeklyPlan.PlannedMeals.Where(pm => pm.DayOfWeek == DayOfWeekType.Tuesday)
                .Select(pm => PlannedMealMapper.ToResponseDto(pm)).ToList(),
                Wednesday = weeklyPlan.PlannedMeals.Where(pm => pm.DayOfWeek == DayOfWeekType.Wednesday)
                .Select(pm => PlannedMealMapper.ToResponseDto(pm)).ToList(),
                Thursday = weeklyPlan.PlannedMeals.Where(pm => pm.DayOfWeek == DayOfWeekType.Thursday)
                .Select(pm => PlannedMealMapper.ToResponseDto(pm)).ToList(),
                Friday = weeklyPlan.PlannedMeals.Where(pm => pm.DayOfWeek == DayOfWeekType.Friday)
                .Select(pm => PlannedMealMapper.ToResponseDto(pm)).ToList(),
                Saturday = weeklyPlan.PlannedMeals.Where(pm => pm.DayOfWeek == DayOfWeekType.Saturday)
                .Select(pm => PlannedMealMapper.ToResponseDto(pm)).ToList(),
                Sunday = weeklyPlan.PlannedMeals.Where(pm => pm.DayOfWeek == DayOfWeekType.Sunday)
                .Select(pm => PlannedMealMapper.ToResponseDto(pm)).ToList()
            };
        }
    }
}
