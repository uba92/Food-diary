using FoodDiary.Api.Enums;

namespace FoodDiary.Api.Models
{
    public class PlannedMeal
    {
        public int Id { get; set; }
        public DayOfWeekType DayOfWeek { get; set; }
        public int WeeklyPlanId { get; set; }
        public int FoodAlternativeId { get; set; }
        public bool Eaten { get; set; }
        public WeeklyPlan WeeklyPlan { get; set; } = null!;
        public FoodAlternative FoodAlternative { get; set; } = null!;
    }
}
