using FoodDiary.Api.Data;
using FoodDiary.Api.Interfaces;
using FoodDiary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Api.Repositories
{
    public class SymptomEntryRepository : ISymptomEntryRepository
    {
        private readonly FoodDiaryDbContext _context;
        public SymptomEntryRepository(FoodDiaryDbContext context)
        {
            _context = context;
        }

        public async Task<SymptomEntry> AddSymptomEntryAsync(SymptomEntry symptomEntry)
        {
            await _context.SymptomEntries.AddAsync(symptomEntry);
            return symptomEntry;
        }

        public void DeleteSymptomEntry(SymptomEntry symptomEntry)
        {
            _context.SymptomEntries.Remove(symptomEntry);
        }

        public async Task<List<SymptomEntry>> GetAllSymptomEntriesAsync()
        {
            return await _context.SymptomEntries
                .Include(s => s.FoodAlternative)
                .OrderByDescending(s => s.OccurredAt)
                .ToListAsync();
        }

        public async Task<List<SymptomEntry>> GetByRangeAsync(DateTime from, DateTime to)
        {
            return await _context.SymptomEntries
                .Include(s => s.FoodAlternative)
                .Where(s => s.OccurredAt >= from && s.OccurredAt <= to)
                .OrderByDescending(s => s.OccurredAt)
                .ToListAsync();
        }

        public async Task<SymptomEntry?> GetSymptomEntryByIdAsync(int id)
        {
            return await _context.SymptomEntries
                .Include(s => s.FoodAlternative)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
