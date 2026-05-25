namespace FoodDiary.Api.Dtos.Response
{
    public class WeeklyPlanByDayResponseDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<PlannedMealResponseDto> Monday { get; set; } = new();
        public List<PlannedMealResponseDto> Tuesday { get; set; } = new();
        public List<PlannedMealResponseDto> Wednesday { get; set; } = new();
        public List<PlannedMealResponseDto> Thursday { get; set; } = new();
        public List<PlannedMealResponseDto> Friday { get; set; } = new();
        public List<PlannedMealResponseDto> Saturday { get; set; } = new();
        public List<PlannedMealResponseDto> Sunday { get; set; } = new();
    }
}
