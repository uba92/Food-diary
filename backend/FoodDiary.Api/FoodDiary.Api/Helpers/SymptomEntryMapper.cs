using FoodDiary.Api.Dtos.Request;
using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Helpers
{
    public class SymptomEntryMapper
    {
        public static SymptomEntry ToEntity(CreateSymptomEntryRequest request)
        {
            return new SymptomEntry
            {
                OccurredAt = DateTime.SpecifyKind(request.OccurredAt, DateTimeKind.Utc),
                Type = request.Type,
                Severity = request.Severity,
                MealType = request.MealType,
                Notes = request.Notes,
                FoodAlternativeId = request.FoodAlternativeId
            };
        }

        public static SymptomEntryResponseDto ToResponseDto(SymptomEntry symptomEntry)
        {
            return new SymptomEntryResponseDto
            {
                Id = symptomEntry.Id,
                OccurredAt = symptomEntry.OccurredAt,
                Type = symptomEntry.Type,
                Severity = symptomEntry.Severity,
                MealType = symptomEntry.MealType,
                Notes = symptomEntry.Notes,
                FoodAlternativeId = symptomEntry.FoodAlternativeId,
                FoodAlternativeName = symptomEntry.FoodAlternative?.Name
            };
        }
    }
}
