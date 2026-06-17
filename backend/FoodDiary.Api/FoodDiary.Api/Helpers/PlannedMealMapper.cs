using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Helpers
{
    public class PlannedMealMapper
    {
        public static PlannedMeal ToEntity(CreatePlannedMealRequest request)
        {
            return new PlannedMeal
            {
                DayOfWeek = request.DayOfWeek,
                WeeklyPlanId = request.WeeklyPlanId,
                FoodAlternativeId = request.FoodAlternativeId
            };
        }

        public static PlannedMealResponseDto ToResponseDto(PlannedMeal plannedMeal)
        {
            return new PlannedMealResponseDto
            {
                Id = plannedMeal.Id,
                DayOfWeek = plannedMeal.DayOfWeek,
                FoodAlternativeId = plannedMeal.FoodAlternativeId,
                WeeklyPlanId = plannedMeal.WeeklyPlanId,
                Eaten = plannedMeal.Eaten,
                FoodAlternativeName = plannedMeal.FoodAlternative != null ? plannedMeal.FoodAlternative.Name : string.Empty,
                MealType = plannedMeal.FoodAlternative != null ? plannedMeal.FoodAlternative.MealType : string.Empty,
                Quantity = plannedMeal.FoodAlternative != null ? plannedMeal.FoodAlternative.Quantity : string.Empty
            };
        }
    }
}
