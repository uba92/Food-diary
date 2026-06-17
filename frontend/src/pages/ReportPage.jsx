import { useCallback, useEffect, useState } from "react";
import { getReport, downloadReportPdf } from "../services/reportService";

// Default: ultime due settimane.
function defaultRange() {
    const today = new Date();
    const from = new Date();
    from.setDate(today.getDate() - 14);
    return {
        from: from.toISOString().slice(0, 10),
        to: today.toISOString().slice(0, 10),
    };
}

const fmtDate = (iso) => new Date(iso).toLocaleDateString();

function ReportPage() {
    const [range, setRange] = useState(defaultRange);
    const [report, setReport] = useState(null);
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(true);

    const fetchReport = useCallback(async (from, to) => {
        try {
            const data = await getReport(
                `${from}T00:00:00`,
                `${to}T23:59:59`
            );
            setReport(data);
            setError("");
        } catch (err) {
            console.error(err);
            setError("Impossibile generare il report.");
        } finally {
            setLoading(false);
        }
    }, []);

    const loadReport = () => {
        if (!range.from || !range.to) {
            setError("Indica entrambe le date.");
            return;
        }
        if (range.from > range.to) {
            setError("La data di inizio deve precedere quella di fine.");
            return;
        }
        setLoading(true);
        fetchReport(range.from, range.to);
    };

    const exportPdf = async () => {
        if (!range.from || !range.to || range.from > range.to) {
            setError("Seleziona un intervallo valido prima di esportare.");
            return;
        }
        setError("");
        try {
            const blob = await downloadReportPdf(
                `${range.from}T00:00:00`,
                `${range.to}T23:59:59`
            );
            const url = URL.createObjectURL(blob);
            const link = document.createElement("a");
            link.href = url;
            link.download = `report-sintomi-${range.from}_${range.to}.pdf`;
            document.body.appendChild(link);
            link.click();
            link.remove();
            URL.revokeObjectURL(url);
        } catch (err) {
            console.error(err);
            setError("Impossibile esportare il PDF.");
        }
    };

    useEffect(() => {
        const run = async () => {
            await fetchReport(range.from, range.to);
        };
        run();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const isEmpty =
        report &&
        report.days.length === 0 &&
        report.symptomFrequency.length === 0 &&
        report.correlations.length === 0;

    return (
        <div className="page">
            <header className="page-header">
                <h1>Report</h1>
                <p>
                    Riepilogo di alimenti e sintomi nel periodo, da condividere
                    con la nutrizionista.
                </p>
            </header>

            <section className="card">
                <div className="form-grid">
                    <div className="field">
                        <label htmlFor="from">Dal</label>
                        <input
                            id="from"
                            type="date"
                            value={range.from}
                            onChange={(e) =>
                                setRange({ ...range, from: e.target.value })
                            }
                        />
                    </div>
                    <div className="field">
                        <label htmlFor="to">Al</label>
                        <input
                            id="to"
                            type="date"
                            value={range.to}
                            onChange={(e) =>
                                setRange({ ...range, to: e.target.value })
                            }
                        />
                    </div>
                    <div className="form-actions">
                        <button
                            type="button"
                            className="btn btn-primary"
                            onClick={loadReport}
                        >
                            Genera report
                        </button>
                        <button
                            type="button"
                            className="btn btn-ghost"
                            onClick={exportPdf}
                        >
                            Esporta PDF
                        </button>
                    </div>
                </div>
                {error && <div className="alert alert-error">{error}</div>}
            </section>

            {loading && <div className="empty-state">Generazione…</div>}

            {!loading && report && isEmpty && (
                <div className="empty-state">
                    <span className="empty-state-icon">📭</span>
                    Nessun dato nel periodo selezionato.
                </div>
            )}

            {!loading && report && !isEmpty && (
                <>
                    <div className="alert alert-info">
                        Le correlazioni indicano <strong>co-occorrenza</strong>{" "}
                        (alimento mangiato e sintomo nello stesso giorno), non un
                        rapporto di causa-effetto. Documento informativo, non
                        sostituisce il parere del professionista.
                    </div>

                    {report.correlations.length > 0 && (
                        <section className="card">
                            <h2 className="card-title">
                                Correlazioni alimento ↔ sintomo
                            </h2>
                            <table className="report-table">
                                <thead>
                                    <tr>
                                        <th>Alimento</th>
                                        <th>Giorni con sintomo</th>
                                        <th>Giorni mangiato</th>
                                        <th>Sintomi</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {report.correlations.map((c) => (
                                        <tr key={c.foodName}>
                                            <td>{c.foodName}</td>
                                            <td>{c.daysWithSymptom}</td>
                                            <td>{c.daysEaten}</td>
                                            <td>
                                                {c.symptomTypes.join(", ") || "—"}
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </section>
                    )}

                    {report.symptomFrequency.length > 0 && (
                        <section className="card">
                            <h2 className="card-title">Frequenza sintomi</h2>
                            <table className="report-table">
                                <thead>
                                    <tr>
                                        <th>Sintomo</th>
                                        <th>Episodi</th>
                                        <th>Severità media</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {report.symptomFrequency.map((s) => (
                                        <tr key={s.type}>
                                            <td>{s.type}</td>
                                            <td>{s.count}</td>
                                            <td>{s.averageSeverity}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </section>
                    )}

                    <section className="card">
                        <h2 className="card-title">Diario giornaliero</h2>
                        <div className="timeline">
                            {report.days.map((d) => (
                                <div key={d.date} className="timeline-day">
                                    <div className="timeline-date">
                                        {fmtDate(d.date)}
                                    </div>
                                    <div className="timeline-body">
                                        <div className="timeline-row">
                                            <span className="timeline-label">
                                                Mangiato
                                            </span>
                                            <span>
                                                {d.foods.length > 0
                                                    ? d.foods.join(", ")
                                                    : "—"}
                                            </span>
                                        </div>
                                        <div className="timeline-row">
                                            <span className="timeline-label">
                                                Sintomi
                                            </span>
                                            <span>
                                                {d.symptoms.length > 0
                                                    ? d.symptoms
                                                          .map(
                                                              (s) =>
                                                                  `${s.type} (${s.severity}, ${s.time})`
                                                          )
                                                          .join(", ")
                                                    : "—"}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </section>
                </>
            )}
        </div>
    );
}

export default ReportPage;
