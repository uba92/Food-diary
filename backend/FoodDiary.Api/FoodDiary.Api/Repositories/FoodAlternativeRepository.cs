using FoodDiary.Api.Data;
using FoodDiary.Api.Interfaces;
using FoodDiary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Api.Repositories
{
    public class FoodAlternativeRepository : IFoodAlternativeRepository
    {
        private readonly FoodDiaryDbContext _context;
        public FoodAlternativeRepository(FoodDiaryDbContext context)
        {
            _context = context;
        }

        public async Task<FoodAlternative> AddFoodAlternativeAsync(FoodAlternative foodAlternative)
        {
            await _context.FoodAlternatives.AddAsync(foodAlternative);
            return foodAlternative;
        }

        public void DeleteFoodAlternative(FoodAlternative foodAlternative)
        {
            _context.FoodAlternatives.Remove(foodAlternative);
        }

        public async Task<List<FoodAlternative>> GetAllFoodAlternativesAsync()
        {
            return await _context.FoodAlternatives.ToListAsync();
        }

        public async Task<FoodAlternative?> GetFoodAlternativeByIdAsync(int id)
        {
            return await _context.FoodAlternatives.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
