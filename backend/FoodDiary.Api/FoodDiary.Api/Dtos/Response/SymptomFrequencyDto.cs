namespace FoodDiary.Api.Dtos.Response
{
    public class SymptomFrequencyDto
    {
        public string Type { get; set; } = string.Empty;
        public int Count { get; set; }
        public double AverageSeverity { get; set; }
    }
}
