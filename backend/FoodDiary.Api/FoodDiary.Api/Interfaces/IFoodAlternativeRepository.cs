using FoodDiary.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Interfaces
{
    public interface IFoodAlternativeRepository
    {
        Task<List<FoodAlternative>> GetAllFoodAlternativesAsync();
        Task<FoodAlternative?> GetFoodAlternativeByIdAsync(int id);
        Task<FoodAlternative> AddFoodAlternativeAsync(FoodAlternative foodAlternative);
        void DeleteFoodAlternative(FoodAlternative foodAlternative);
        Task SaveChangesAsync();
    }
}
