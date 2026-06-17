namespace FoodDiary.Api.Dtos.Response
{
    public class FoodSymptomCorrelationDto
    {
        public string FoodName { get; set; } = string.Empty;
        public int DaysEaten { get; set; }
        public int DaysWithSymptom { get; set; }
        public List<string> SymptomTypes { get; set; } = new();
    }
}
