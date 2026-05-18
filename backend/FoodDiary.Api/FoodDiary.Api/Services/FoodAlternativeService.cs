using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Helpers;
using FoodDiary.Api.Interfaces;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Services
{
    public class FoodAlternativeService : IFoodAlternativeService
    {
        private readonly IFoodAlternativeRepository _repository;
        public FoodAlternativeService(IFoodAlternativeRepository repository)
        {
            _repository = repository;
        }

        public async Task<FoodAlternativeResponseDto> CreateAsync(CreateFoodAlternativeRequest request)
        {
            var requestToEntity = FoodAlternativeMapper.ToEntity(request);

            await _repository.AddFoodAlternativeAsync(requestToEntity);
            await _repository.SaveChangesAsync();
            return FoodAlternativeMapper.ToResponseDto(requestToEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entityToDelete =  await _repository.GetFoodAlternativeByIdAsync(id);
            if(entityToDelete == null)
            {
                return false;
            }
            _repository.DeleteFoodAlternative(entityToDelete);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<List<FoodAlternativeResponseDto>> GetAllAsync()
        {
            var foodAlternatives = await _repository.GetAllFoodAlternativesAsync();
            var foodAlternativesDto = new List<FoodAlternativeResponseDto>();
            foreach (var foodAlternative in foodAlternatives)
            {
                var response = FoodAlternativeMapper.ToResponseDto(foodAlternative);
                foodAlternativesDto.Add(response);
            }
            return foodAlternativesDto;
        }

        public async Task<FoodAlternativeResponseDto?> GetByIdAsync(int id)
        {
            var foodAlternative = await _repository.GetFoodAlternativeByIdAsync(id);
            if (foodAlternative == null)
            {
                return null;
            }
            var response = FoodAlternativeMapper.ToResponseDto(foodAlternative);
            return response;
        }

        public async Task<FoodAlternativeResponseDto?> UpdateAsync(int id, UpdateFoodAlternativeRequest request)
        {
            var entityToUpdate = await _repository.GetFoodAlternativeByIdAsync(id);
            if(entityToUpdate == null)
            {
                return null;
            }
            entityToUpdate.Name = request.Name;
            entityToUpdate.MealType = request.MealType;
            entityToUpdate.Quantity = request.Quantity;
            entityToUpdate.WeeklyFrequency = request.WeeklyFrequency;
            entityToUpdate.Notes = request.Notes;

            await _repository.SaveChangesAsync();
            return FoodAlternativeMapper.ToResponseDto(entityToUpdate);
        }
    }
}
