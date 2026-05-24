namespace FoodDiary.Api.Dtos.Request
{
    public class UpdateWeeklyPlanRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
