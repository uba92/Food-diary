import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import {
    getAllWeeklyPlans,
    createWeeklyPlan,
    updateWeeklyPlan,
    deleteWeeklyPlan,
} from "../services/weeklyPlanService";
import ConfirmDialog from "../components/ConfirmDialog";

const emptyForm = {
    startDate: "",
    endDate: "",
};

function WeeklyPlansPage() {
    const [weeklyPlans, setWeeklyPlans] = useState([]);
    const [form, setForm] = useState(emptyForm);
    const [editingId, setEditingId] = useState(null);
    const [toDelete, setToDelete] = useState(null);
    const [error, setError] = useState("");

    const handleChange = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value,
        });
    };

    const resetForm = () => {
        setForm(emptyForm);
        setEditingId(null);
    };

    const startEdit = (plan) => {
        setEditingId(plan.id);
        setForm({
            startDate: plan.startDate.slice(0, 10),
            endDate: plan.endDate.slice(0, 10),
        });
        setError("");
        window.scrollTo({ top: 0, behavior: "smooth" });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");

        if (!form.startDate || !form.endDate) {
            setError("Indica sia la data di inizio che quella di fine.");
            return;
        }
        if (form.startDate >= form.endDate) {
            setError("La data di inizio deve essere precedente alla data di fine.");
            return;
        }

        try {
            if (editingId) {
                const updated = await updateWeeklyPlan(editingId, form);
                setWeeklyPlans(
                    weeklyPlans.map((p) => (p.id === editingId ? updated : p))
                );
            } else {
                const created = await createWeeklyPlan(form);
                setWeeklyPlans([...weeklyPlans, created]);
            }
            resetForm();
        } catch (err) {
            console.error(err);
            setError("Impossibile salvare il piano. Riprova.");
        }
    };

    const confirmDelete = async () => {
        const id = toDelete.id;
        setToDelete(null);
        try {
            await deleteWeeklyPlan(id);
            setWeeklyPlans(weeklyPlans.filter((p) => p.id !== id));
            if (editingId === id) resetForm();
        } catch (err) {
            console.error(err);
            setError("Impossibile eliminare il piano. Riprova.");
        }
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await getAllWeeklyPlans();
                setWeeklyPlans(data);
            } catch (err) {
                console.error(err);
                setError("Impossibile caricare i piani settimanali.");
            }
        };
        fetchData();
    }, []);

    return (
        <div className="page">
            <header className="page-header">
                <h1>Piani settimanali</h1>
                <p>
                    Organizza le tue settimane di pasti e mantieni le tue
                    abitudini in equilibrio.
                </p>
            </header>

            <section className="card">
                <h2 className="card-title">
                    {editingId ? "Modifica piano" : "Nuovo piano"}
                </h2>
                <form onSubmit={handleSubmit}>
                    <div className="form-grid">
                        <div className="field">
                            <label htmlFor="startDate">Data di inizio</label>
                            <input
                                id="startDate"
                                type="date"
                                name="startDate"
                                value={form.startDate}
                                onChange={handleChange}
                            />
                        </div>
                        <div className="field">
                            <label htmlFor="endDate">Data di fine</label>
                            <input
                                id="endDate"
                                type="date"
                                name="endDate"
                                value={form.endDate}
                                onChange={handleChange}
                            />
                        </div>
                        <div className="form-actions">
                            <button type="submit" className="btn btn-primary">
                                {editingId ? "Salva modifiche" : "Crea piano"}
                            </button>
                            {editingId && (
                                <button
                                    type="button"
                                    className="btn btn-ghost"
                                    onClick={resetForm}
                                >
                                    Annulla
                                </button>
                            )}
                        </div>
                    </div>
                </form>
                {error && <div className="alert alert-error">{error}</div>}
            </section>

            {weeklyPlans.length === 0 ? (
                <div className="empty-state">
                    <span className="empty-state-icon">🗓️</span>
                    Nessun piano ancora. Crea il tuo primo piano settimanale.
                </div>
            ) : (
                <div className="list">
                    {weeklyPlans.map((plan) => (
                        <div key={plan.id} className="list-item">
                            <Link
                                to={`/weekly-plans/${plan.id}`}
                                className="item-body item-link"
                            >
                                <div className="item-title">
                                    Settimana dal{" "}
                                    {new Date(plan.startDate).toLocaleDateString()}
                                </div>
                                <div className="item-sub">
                                    al{" "}
                                    {new Date(plan.endDate).toLocaleDateString()}
                                </div>
                            </Link>
                            <span className="chip">
                                {plan.plannedMeals?.length ?? 0} pasti
                            </span>
                            <div className="item-actions">
                                <button
                                    type="button"
                                    className="btn-icon"
                                    title="Modifica"
                                    onClick={() => startEdit(plan)}
                                >
                                    ✎
                                </button>
                                <button
                                    type="button"
                                    className="btn-icon-danger"
                                    title="Elimina"
                                    onClick={() => setToDelete(plan)}
                                >
                                    ✕
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            <ConfirmDialog
                open={toDelete !== null}
                title="Elimina piano"
                message={
                    toDelete
                        ? "Vuoi eliminare questo piano settimanale e tutti i suoi pasti? L'azione non è reversibile."
                        : ""
                }
                confirmLabel="Elimina"
                onConfirm={confirmDelete}
                onCancel={() => setToDelete(null)}
            />
        </div>
    );
}

export default WeeklyPlansPage;
