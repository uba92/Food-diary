using FoodDiary.Api.Helpers;
using FoodDiary.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace FoodDiary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReport([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var report = await _reportService.GetReportAsync(from, to);
            return Ok(report);
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetReportPdf([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var report = await _reportService.GetReportAsync(from, to);
            var bytes = new SymptomReportDocument(report).GeneratePdf();
            var fileName = $"report-sintomi-{report.From:yyyyMMdd}-{report.To:yyyyMMdd}.pdf";
            return File(bytes, "application/pdf", fileName);
        }
    }
}
