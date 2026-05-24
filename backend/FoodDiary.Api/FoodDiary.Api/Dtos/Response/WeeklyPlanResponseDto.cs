namespace FoodDiary.Api.Dtos.Response
{
    public class WeeklyPlanResponseDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<PlannedMealResponseDto> PlannedMeals { get; set; } = new List<PlannedMealResponseDto>();
    }
}
