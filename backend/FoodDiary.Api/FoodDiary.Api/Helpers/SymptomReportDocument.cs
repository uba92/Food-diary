using FoodDiary.Api.Dtos.Response;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FoodDiary.Api.Helpers
{
    public class SymptomReportDocument : IDocument
    {
        private readonly SymptomReportDto _report;
        private static readonly string Primary = "#1f7d56";
        private static readonly string Muted = "#6b7d74";

        public SymptomReportDocument(SymptomReportDto report)
        {
            _report = report;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(ComposeHeader);
                page.Content().PaddingVertical(15).Element(ComposeContent);
                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("FoodDiary · ").FontColor(Muted);
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Report sintomi e alimenti")
                    .FontSize(18).Bold().FontColor(Primary);
                column.Item().Text(
                    $"Periodo dal {_report.From:dd/MM/yyyy} al {_report.To:dd/MM/yyyy}")
                    .FontColor(Muted);
                column.Item().Text($"Generato il {DateTime.Now:dd/MM/yyyy}")
                    .FontSize(8).FontColor(Muted);
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(18);

                column.Item().Background("#e9f3fb").Padding(8).Text(
                    "Le correlazioni indicano una co-occorrenza (alimento mangiato e " +
                    "sintomo nello stesso giorno), non un rapporto di causa-effetto. " +
                    "Documento informativo, non sostituisce il parere del professionista.")
                    .FontSize(8).FontColor("#2c5d80");

                ComposeCorrelations(column);
                ComposeFrequency(column);
                ComposeTimeline(column);
            });
        }

        private void ComposeCorrelations(ColumnDescriptor column)
        {
            column.Item().Text("Correlazioni alimento ↔ sintomo").FontSize(13).Bold();
            if (_report.Correlations.Count == 0)
            {
                column.Item().Text("Nessun dato.").FontColor(Muted);
                return;
            }
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.RelativeColumn(3);
                    c.RelativeColumn();
                    c.RelativeColumn();
                    c.RelativeColumn(3);
                });
                table.Header(header =>
                {
                    HeaderCell(header, "Alimento");
                    HeaderCell(header, "Gg. con sintomo");
                    HeaderCell(header, "Gg. mangiato");
                    HeaderCell(header, "Sintomi");
                });
                foreach (var c in _report.Correlations)
                {
                    BodyCell(table, c.FoodName);
                    BodyCell(table, c.DaysWithSymptom.ToString());
                    BodyCell(table, c.DaysEaten.ToString());
                    BodyCell(table, c.SymptomTypes.Count > 0 ? string.Join(", ", c.SymptomTypes) : "—");
                }
            });
        }

        private void ComposeFrequency(ColumnDescriptor column)
        {
            column.Item().Text("Frequenza sintomi").FontSize(13).Bold();
            if (_report.SymptomFrequency.Count == 0)
            {
                column.Item().Text("Nessun sintomo registrato.").FontColor(Muted);
                return;
            }
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.RelativeColumn(3);
                    c.RelativeColumn();
                    c.RelativeColumn();
                });
                table.Header(header =>
                {
                    HeaderCell(header, "Sintomo");
                    HeaderCell(header, "Episodi");
                    HeaderCell(header, "Severità media");
                });
                foreach (var s in _report.SymptomFrequency)
                {
                    BodyCell(table, s.Type);
                    BodyCell(table, s.Count.ToString());
                    BodyCell(table, s.AverageSeverity.ToString("0.#"));
                }
            });
        }

        private void ComposeTimeline(ColumnDescriptor column)
        {
            column.Item().Text("Diario giornaliero").FontSize(13).Bold();
            if (_report.Days.Count == 0)
            {
                column.Item().Text("Nessun dato nel periodo.").FontColor(Muted);
                return;
            }
            column.Item().Column(days =>
            {
                days.Spacing(8);
                foreach (var d in _report.Days)
                {
                    days.Item().BorderBottom(1).BorderColor("#e1ece6").PaddingBottom(6).Column(day =>
                    {
                        day.Item().Text(d.Date.ToString("dddd dd/MM/yyyy")).Bold().FontColor(Primary);
                        day.Item().Text(text =>
                        {
                            text.Span("Mangiato: ").SemiBold();
                            text.Span(d.Foods.Count > 0 ? string.Join(", ", d.Foods) : "—");
                        });
                        day.Item().Text(text =>
                        {
                            text.Span("Sintomi: ").SemiBold();
                            text.Span(d.Symptoms.Count > 0
                                ? string.Join(", ", d.Symptoms.Select(s => $"{s.Type} (sev. {s.Severity}, {s.Time})"))
                                : "—");
                        });
                    });
                }
            });
        }

        private static void HeaderCell(TableCellDescriptor header, string text)
        {
            header.Cell().Background("#e3f3ea").Padding(5).Text(text)
                .FontSize(8).Bold().FontColor("#1f7d56");
        }

        private static void BodyCell(TableDescriptor table, string text)
        {
            table.Cell().BorderBottom(1).BorderColor("#e1ece6").Padding(5).Text(text).FontSize(9);
        }
    }
}
