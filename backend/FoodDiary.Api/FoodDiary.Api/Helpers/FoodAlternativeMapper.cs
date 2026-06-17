using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Helpers
{
    public class FoodAlternativeMapper
    {
        public static FoodAlternative ToEntity(CreateFoodAlternativeRequest request)
        {
            return new FoodAlternative
            {
                Name = request.Name,
                MealType = request.MealType,
                Quantity = request.Quantity,
                WeeklyFrequency = request.WeeklyFrequency,
                Notes = request.Notes,
                FoodCategory = request.FoodCategory
            };
        }

        public static FoodAlternativeResponseDto ToResponseDto(FoodAlternative foodAlternative)
        {
            return new FoodAlternativeResponseDto
            {
                Id = foodAlternative.Id,
                Name = foodAlternative.Name,
                MealType = foodAlternative.MealType,
                Quantity = foodAlternative.Quantity,
                WeeklyFrequency = foodAlternative.WeeklyFrequency,
                Notes = foodAlternative.Notes,
                FoodCategory = foodAlternative.FoodCategory
            };
        }
    }
}
