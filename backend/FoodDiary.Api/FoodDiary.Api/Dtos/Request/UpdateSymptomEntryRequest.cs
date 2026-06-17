namespace FoodDiary.Api.Dtos.Request
{
    public class UpdateSymptomEntryRequest
    {
        public DateTime OccurredAt { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Severity { get; set; }
        public string? MealType { get; set; }
        public string? Notes { get; set; }
        public int? FoodAlternativeId { get; set; }
    }
}
