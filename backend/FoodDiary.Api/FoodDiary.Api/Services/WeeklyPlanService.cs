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
            var requestToEntity = WeeklyPlanMapper.ToEntity(request);
            await _weeklyPlanRepository.AddWeeklyPlanAsync(requestToEntity);
            await _weeklyPlanRepository.SaveChangesAsync();
            return WeeklyPlanMapper.ToResponseDto(requestToEntity);
        }

        public async Task<bool> DeleteWeeklyPlanAsync(int id)
        {
            var entityToDelete = await _weeklyPlanRepository.GetWeeklyPLanByIdAsync(id);
            if(entityToDelete == null)
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
            foreach(var w in weeklyPlans)
            {
                var response = WeeklyPlanMapper.ToResponseDto(w);
                weeklyPlansDto.Add(response);
            }
            return weeklyPlansDto;
        }

        public async Task<WeeklyPlanResponseDto?> GetWeeklyPlanByIdAsync(int id)
        {
            var weeklyPlan = await _weeklyPlanRepository.GetWeeklyPLanByIdAsync(id);
            if(weeklyPlan == null)
            {
                return null;
            }
            return WeeklyPlanMapper.ToResponseDto(weeklyPlan);
        }

        public async Task<bool> UpdateWeeklyPlanAsync(int id, UpdateWeeklyPlanRequest request)
        {
            var entityToUpdate = await _weeklyPlanRepository.GetWeeklyPLanByIdAsync(id);
            if(entityToUpdate == null)
            {
                return false;
            }
            entityToUpdate.StartDate = request.StartDate;
            entityToUpdate.EndDate = request.EndDate;
            await _weeklyPlanRepository.SaveChangesAsync();
            return true;
        }
    }
}
