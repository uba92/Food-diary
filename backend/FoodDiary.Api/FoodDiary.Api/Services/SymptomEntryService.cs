using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Helpers;
using FoodDiary.Api.Interfaces;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Services
{
    public class SymptomEntryService : ISymptomEntryService
    {
        private readonly ISymptomEntryRepository _symptomEntryRepository;
        private readonly IFoodAlternativeRepository _foodAlternativeRepository;
        public SymptomEntryService(ISymptomEntryRepository symptomEntryRepository, IFoodAlternativeRepository foodAlternativeRepository)
        {
            _symptomEntryRepository = symptomEntryRepository;
            _foodAlternativeRepository = foodAlternativeRepository;
        }

        public async Task<List<SymptomEntryResponseDto>> GetAllAsync(DateTime? from, DateTime? to)
        {
            List<SymptomEntry> entries;
            if (from.HasValue && to.HasValue)
            {
                var fromUtc = DateTime.SpecifyKind(from.Value, DateTimeKind.Utc);
                var toUtc = DateTime.SpecifyKind(to.Value, DateTimeKind.Utc);
                entries = await _symptomEntryRepository.GetByRangeAsync(fromUtc, toUtc);
            }
            else
            {
                entries = await _symptomEntryRepository.GetAllSymptomEntriesAsync();
            }
            return entries.Select(SymptomEntryMapper.ToResponseDto).ToList();
        }

        public async Task<SymptomEntryResponseDto?> GetByIdAsync(int id)
        {
            var entry = await _symptomEntryRepository.GetSymptomEntryByIdAsync(id);
            if (entry == null)
            {
                return null;
            }
            return SymptomEntryMapper.ToResponseDto(entry);
        }

        public async Task<SymptomEntryResponseDto> CreateAsync(CreateSymptomEntryRequest request)
        {
            await ValidateAsync(request.OccurredAt, request.Type, request.Severity, request.FoodAlternativeId);

            var entry = SymptomEntryMapper.ToEntity(request);
            if (request.FoodAlternativeId.HasValue)
            {
                entry.FoodAlternative = await _foodAlternativeRepository.GetFoodAlternativeByIdAsync(request.FoodAlternativeId.Value);
            }
            await _symptomEntryRepository.AddSymptomEntryAsync(entry);
            await _symptomEntryRepository.SaveChangesAsync();
            return SymptomEntryMapper.ToResponseDto(entry);
        }

        public async Task<SymptomEntryResponseDto?> UpdateAsync(int id, UpdateSymptomEntryRequest request)
        {
            var entry = await _symptomEntryRepository.GetSymptomEntryByIdAsync(id);
            if (entry == null)
            {
                return null;
            }
            await ValidateAsync(request.OccurredAt, request.Type, request.Severity, request.FoodAlternativeId);

            entry.OccurredAt = DateTime.SpecifyKind(request.OccurredAt, DateTimeKind.Utc);
            entry.Type = request.Type;
            entry.Severity = request.Severity;
            entry.MealType = request.MealType;
            entry.Notes = request.Notes;
            entry.FoodAlternativeId = request.FoodAlternativeId;
            entry.FoodAlternative = request.FoodAlternativeId.HasValue
                ? await _foodAlternativeRepository.GetFoodAlternativeByIdAsync(request.FoodAlternativeId.Value)
                : null;

            await _symptomEntryRepository.SaveChangesAsync();
            return SymptomEntryMapper.ToResponseDto(entry);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _symptomEntryRepository.GetSymptomEntryByIdAsync(id);
            if (entry == null)
            {
                return false;
            }
            _symptomEntryRepository.DeleteSymptomEntry(entry);
            await _symptomEntryRepository.SaveChangesAsync();
            return true;
        }

        private async Task ValidateAsync(DateTime occurredAt, string type, int severity, int? foodAlternativeId)
        {
            if (occurredAt == default)
            {
                throw new ArgumentException("OccurredAt is required.");
            }
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("Symptom type is required.");
            }
            if (severity < 1 || severity > 5)
            {
                throw new ArgumentException("Severity must be between 1 and 5.");
            }
            if (foodAlternativeId.HasValue)
            {
                var food = await _foodAlternativeRepository.GetFoodAlternativeByIdAsync(foodAlternativeId.Value);
                if (food == null)
                {
                    throw new ArgumentException("Invalid FoodAlternativeId.");
                }
            }
        }
    }
}
