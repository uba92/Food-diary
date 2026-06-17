namespace FoodDiary.Api.Dtos.Response
{
    public class ReportDayDto
    {
        public DateTime Date { get; set; }
        public List<string> Foods { get; set; } = new();
        public List<ReportSymptomDto> Symptoms { get; set; } = new();
    }
}
