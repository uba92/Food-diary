namespace FoodDiary.Api.Dtos.Request
{
    public class CreateFoodAlternativeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string MealType { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public int WeeklyFrequency { get; set; }
        public string? Notes { get; set; }
        public string? FoodCategory { get; set; }
    }
}
