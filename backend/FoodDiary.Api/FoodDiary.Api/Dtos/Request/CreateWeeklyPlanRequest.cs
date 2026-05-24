namespace FoodDiary.Api.Dtos.Request
{
    public class CreateWeeklyPlanRequest
    {
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
    }
}
