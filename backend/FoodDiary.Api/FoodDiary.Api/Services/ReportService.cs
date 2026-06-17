using FoodDiary.Api.Dtos.Response;
using FoodDiary.Api.Interfaces;
using FoodDiary.Api.Models;

namespace FoodDiary.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly IWeeklyPlanRepository _weeklyPlanRepository;
        private readonly ISymptomEntryRepository _symptomEntryRepository;
        public ReportService(IWeeklyPlanRepository weeklyPlanRepository, ISymptomEntryRepository symptomEntryRepository)
        {
            _weeklyPlanRepository = weeklyPlanRepository;
            _symptomEntryRepository = symptomEntryRepository;
        }

        public async Task<SymptomReportDto> GetReportAsync(DateTime from, DateTime to)
        {
            var fromUtc = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            var toUtc = DateTime.SpecifyKind(to, DateTimeKind.Utc);
            if (fromUtc > toUtc)
            {
                throw new ArgumentException("'from' cannot be later than 'to'.");
            }

            var plans = await _weeklyPlanRepository.GetAllWeeklyPlansAsync();
            var symptoms = await _symptomEntryRepository.GetByRangeAsync(fromUtc, toUtc);

            // Alimenti effettivamente mangiati, raggruppati per data (giorno del piano + offset).
            var eatenByDate = new Dictionary<DateTime, List<FoodAlternative>>();
            foreach (var plan in plans)
            {
                foreach (var meal in plan.PlannedMeals)
                {
                    if (!meal.Eaten || meal.FoodAlternative == null)
                    {
                        continue;
                    }
                    var date = plan.StartDate.Date.AddDays((int)meal.DayOfWeek);
                    if (date < fromUtc.Date || date > toUtc.Date)
                    {
                        continue;
                    }
                    if (!eatenByDate.TryGetValue(date, out var foods))
                    {
                        foods = new List<FoodAlternative>();
                        eatenByDate[date] = foods;
                    }
                    foods.Add(meal.FoodAlternative);
                }
            }

            var symptomsByDate = symptoms
                .GroupBy(s => s.OccurredAt.Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Timeline giornaliera: unione dei giorni con cibo mangiato e/o sintomi.
            var allDates = eatenByDate.Keys
                .Union(symptomsByDate.Keys)
                .OrderBy(d => d)
                .ToList();

            var days = allDates.Select(date => new ReportDayDto
            {
                Date = date,
                Foods = eatenByDate.TryGetValue(date, out var f)
                    ? f.Select(x => x.Name).Distinct().ToList()
                    : new List<string>(),
                Symptoms = symptomsByDate.TryGetValue(date, out var s)
                    ? s.Select(x => new ReportSymptomDto
                    {
                        Type = x.Type,
                        Severity = x.Severity,
                        Time = x.OccurredAt.ToString("HH:mm"),
                        FoodAlternativeName = x.FoodAlternative?.Name
                    }).ToList()
                    : new List<ReportSymptomDto>()
            }).ToList();

            // Frequenza per tipo di sintomo + severità media.
            var frequency = symptoms
                .GroupBy(s => s.Type)
                .Select(g => new SymptomFrequencyDto
                {
                    Type = g.Key,
                    Count = g.Count(),
                    AverageSeverity = Math.Round(g.Average(x => x.Severity), 1)
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            // Correlazioni alimento↔sintomo (co-occorrenza per giorno, NON causalità).
            var datesByFood = new Dictionary<string, HashSet<DateTime>>();
            foreach (var (date, foods) in eatenByDate)
            {
                foreach (var food in foods)
                {
                    if (!datesByFood.TryGetValue(food.Name, out var set))
                    {
                        set = new HashSet<DateTime>();
                        datesByFood[food.Name] = set;
                    }
                    set.Add(date);
                }
            }

            var correlations = datesByFood.Select(kv =>
            {
                var symptomDays = kv.Value.Where(d => symptomsByDate.ContainsKey(d)).ToList();
                var types = symptomDays
                    .SelectMany(d => symptomsByDate[d])
                    .Select(x => x.Type)
                    .Distinct()
                    .OrderBy(t => t)
                    .ToList();
                return new FoodSymptomCorrelationDto
                {
                    FoodName = kv.Key,
                    DaysEaten = kv.Value.Count,
                    DaysWithSymptom = symptomDays.Count,
                    SymptomTypes = types
                };
            })
            .OrderByDescending(c => c.DaysWithSymptom)
            .ThenByDescending(c => c.DaysEaten)
            .ToList();

            return new SymptomReportDto
            {
                From = fromUtc,
                To = toUtc,
                Days = days,
                SymptomFrequency = frequency,
                Correlations = correlations
            };
        }
    }
}
