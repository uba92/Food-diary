namespace FoodDiary.Api.Models
{
    public class WeeklyPlan
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<PlannedMeal> PlannedMeals { get; set; } = new List<PlannedMeal>();
    }
}
