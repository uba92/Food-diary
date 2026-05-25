namespace FoodDiary.Api.Dtos.Response
{
    public class FoodAlternativeUsageStatsDto
    {
        public int FoodAlternativeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int WeeklyFrequency { get; set; }
        public int UsedCount { get; set; }
        public bool IsOverLimited { get; set; }
    }
}
