namespace FoodDiary.Api.Dtos.Response
{
    public class ReportSymptomDto
    {
        public string Type { get; set; } = string.Empty;
        public int Severity { get; set; }
        public string Time { get; set; } = string.Empty;
        public string? FoodAlternativeName { get; set; }
    }
}
