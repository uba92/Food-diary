namespace FoodDiary.Api.Dtos.Response
{
    public class SymptomReportDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<ReportDayDto> Days { get; set; } = new();
        public List<SymptomFrequencyDto> SymptomFrequency { get; set; } = new();
        public List<FoodSymptomCorrelationDto> Correlations { get; set; } = new();
    }
}
