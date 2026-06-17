namespace FoodDiary.Api.Models
{
    public class SymptomEntry
    {
        public int Id { get; set; }
        public DateTime OccurredAt { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Severity { get; set; }
        public string? MealType { get; set; }
        public string? Notes { get; set; }
        public int? FoodAlternativeId { get; set; }
        public FoodAlternative? FoodAlternative { get; set; }
    }
}
