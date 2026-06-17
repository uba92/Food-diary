using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Helpers;
using FoodDiary.Api.Interfaces;

namespace FoodDiary.Api.Services
{
    public class WeeklyPlanService : IWeeklyPlanService
    {
        private readonly IWeeklyPlanRepository _weeklyPlanRepository;
        public WeeklyPlanService(IWeeklyPlanRepository weeklyPlanRepository)
        {
            _weeklyPlanRepository = weeklyPlanRepository;
        }

        public async Task<WeeklyPlanResponseDto> CreateWeeklyPlanAsync(CreateWeeklyPlanRequest request)
        {
            if (request.StartDate >= request.EndDate)
            {
                throw new ArgumentException("Start date cannot be later than end date.");
            }
            var requestToEntity = WeeklyPlanMapper.ToEntity(request);
            await _weeklyPlanRepository.AddWeeklyPlanAsync(requestToEntity);
            await _weeklyPlanRepository.SaveChangesAsync();
            return WeeklyPlanMapper.ToResponseDto(requestToEntity);
        }

        public async Task<bool> DeleteWeeklyPlanAsync(int id)
        {
            var entityToDelete = await _weeklyPlanRepository.GetWeeklyPlanByIdAsync(id);
            if (entityToDelete == null)
            {
                return false;
            }
            _weeklyPlanRepository.DeleteWeeklyPlan(entityToDelete);
            await _weeklyPlanRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<WeeklyPlanResponseDto>> GetAllWeeklyPlansAsync()
        {
            var weeklyPlans = await _weeklyPlanRepository.GetAllWeeklyPlansAsync();
            var weeklyPlansDto = new List<WeeklyPlanResponseDto>();
            foreach (var w in weeklyPlans)
            {
                var response = WeeklyPlanMapper.ToResponseDto(w);
                weeklyPlansDto.Add(response);
            }
            return weeklyPlansDto;
        }

        public async Task<WeeklyPlanResponseDto?> GetWeeklyPlanByIdAsync(int id)
        {
            var weeklyPlan = await _weeklyPlanRepository.GetWeeklyPlanByIdAsync(id);
            if (weeklyPlan == null)
            {
                return null;
            }
            return WeeklyPlanMapper.ToResponseDto(weeklyPlan);
        }

        public async Task<WeeklyPlanResponseDto?> UpdateWeeklyPlanAsync(int id, UpdateWeeklyPlanRequest request)
        {
            var entityToUpdate = await _weeklyPlanRepository.GetWeeklyPlanByIdAsync(id);
            if (entityToUpdate == null)
            {
                return null;
            }
            if (request.StartDate >= request.EndDate)
            {
                throw new ArgumentException("Start date cannot be later than end date.");
            }
            entityToUpdate.StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc);
            entityToUpdate.EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc);
            await _weeklyPlanRepository.SaveChangesAsync();
            return WeeklyPlanMapper.ToResponseDto(entityToUpdate);
        }

        public async Task<WeeklyPlanByDayResponseDto?> GetWeeklyPlanByIdWithMealsByDayAsync(int id)
        {
            var weeklyPlan = await _weeklyPlanRepository.GetWeeklyPlanByIdAsync(id);
            if (weeklyPlan == null)
            {
                return null;
            }
            return WeeklyPlanMapper.ToByDayResponseDto(weeklyPlan);
        }

        public async Task<List<FoodAlternativeUsageStatsDto>?> GetFoodAlternativeUsageStatsAsync(int id)
        {
            var weeklyPlan = await _weeklyPlanRepository.GetWeeklyPlanByIdAsync(id);
            if(weeklyPlan == null)
            {
                return null;
            }
            var groupedMeals = weeklyPlan.PlannedMeals
                .GroupBy(pm => pm.FoodAlternativeId);

            var stats = groupedMeals
                .Select(g =>
                {
                    var firstMeal = g.First();
                    var foodAlternative = firstMeal.FoodAlternative;
                    return new FoodAlternativeUsageStatsDto
                    {
                        FoodAlternativeId = g.Key,
                        Name = foodAlternative.Name,
                        WeeklyFrequency = foodAlternative.WeeklyFrequency,
                        UsedCount = g.Count(),
                        IsOverLimited = g.Count() > foodAlternative.WeeklyFrequency
                    };
                }).ToList();
            return stats;
        }
    }
}
