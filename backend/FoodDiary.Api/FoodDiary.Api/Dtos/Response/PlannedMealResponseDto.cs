using FoodDiary.Api.Enums;

namespace FoodDiary.Api.Dtos.Response
{
    public class PlannedMealResponseDto
    {
        public int Id { get; set; }
        public DayOfWeekType DayOfWeek { get; set; }
        public int FoodAlternativeId { get; set; }
        public int WeeklyPlanId { get; set; }
        public string FoodAlternativeName { get; set; } = string.Empty;
        public string MealType { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
    }
}
