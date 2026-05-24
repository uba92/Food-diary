using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Helpers;
using FoodDiary.Api.Interfaces;

namespace FoodDiary.Api.Services
{
    public class PlannedMealService : IPlannedMealService
    {
        private readonly IPlannedMealRepository _plannedMealRepository;
        private readonly IFoodAlternativeRepository _foodAlternativeRepository;
        private readonly IWeeklyPlanRepository _weeklyPlanRepository;
        public PlannedMealService(IPlannedMealRepository plannedMealRepository, IFoodAlternativeRepository foodAlternativeRepository, IWeeklyPlanRepository weeklyPlanRepository)
        {
            _plannedMealRepository = plannedMealRepository;
            _foodAlternativeRepository = foodAlternativeRepository;
            _weeklyPlanRepository = weeklyPlanRepository;
        }
        public async Task<PlannedMealResponseDto?> CreatePlannedMealAsync(CreatePlannedMealRequest request)
        {
            var weeklyPlan = await _weeklyPlanRepository.GetWeeklyPlanByIdAsync(request.WeeklyPlanId);
            var foodAlternative = await _foodAlternativeRepository.GetFoodAlternativeByIdAsync(request.FoodAlternativeId);
            if(foodAlternative == null || weeklyPlan == null)
            {
                return null;
            }
            var meal = await _plannedMealRepository.AddPlannedMealAsync(PlannedMealMapper.ToEntity(request));
            
            await _plannedMealRepository.SaveChangesAsync();
            return PlannedMealMapper.ToResponseDto(meal);
        }

        public async Task<bool> DeletePlannedMealAsync(int id)
        {
            var meal = await _plannedMealRepository.GetPlannedMealByIdAsync(id);
            if(meal == null)
            {
                return false;
            }
            _plannedMealRepository.DeletePlannedMeal(meal);
            await _plannedMealRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<PlannedMealResponseDto>> GetAllPlannedMealsAsync()
        {
            var meals = await _plannedMealRepository.GetAllPlannedMealsAsync();
            return meals.Select(m => PlannedMealMapper.ToResponseDto(m)).ToList();
        }

        public async Task<PlannedMealResponseDto?> GetPlannedMealByIdAsync(int id)
        {
            var meal = await _plannedMealRepository.GetPlannedMealByIdAsync(id);
            if(meal == null)
            {
                return null;
            }
            return PlannedMealMapper.ToResponseDto(meal);
        }

        public async Task<bool> UpdatePlannedMealAsync(int id, UpdatePlannedMealRequest request)
        {
            var meal = await _plannedMealRepository.GetPlannedMealByIdAsync(id);
            if(meal == null)
            {
                return false;
            }
            meal.DayOfWeek = request.DayOfWeek;
            meal.WeeklyPlanId = request.WeeklyPlanId;
            meal.FoodAlternativeId = request.FoodAlternativeId;
            await _plannedMealRepository.SaveChangesAsync();
            return true;
        }
    }
}
