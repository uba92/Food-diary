using FoodDiary.Api.Dtos.Response;

namespace FoodDiary.Api.Interfaces
{
    public interface IReportService
    {
        Task<SymptomReportDto> GetReportAsync(DateTime from, DateTime to);
    }
}
