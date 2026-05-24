using FoodDiary.Api.Enums;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Dtos.Request
{
    public class UpdatePlannedMealRequest
    {
        public DayOfWeekType DayOfWeek { get; set; }
        public int WeeklyPlanId { get; set; }
        public int FoodAlternativeId { get; set; }
    }
}
