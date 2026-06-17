import { useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import {
    getWeeklyPlanByDay,
    getWeeklyPlanUsageStats,
} from "../services/weeklyPlanService";
import { getFoodAlternatives } from "../services/foodAlternativeService";
import {
    createPlannedMeal,
    updatePlannedMeal,
    deletePlannedMeal,
} from "../services/plannedMealService";
import {
    getSymptoms,
    createSymptom,
    updateSymptom,
    deleteSymptom,
} from "../services/symptomService";
import SymptomFormModal from "../components/SymptomFormModal";
import ConfirmDialog from "../components/ConfirmDialog";

// DayOfWeekType lato backend: Monday = 0 ... Sunday = 6.
const DAYS = [
    { key: "monday", label: "Lunedì", value: 0 },
    { key: "tuesday", label: "Martedì", value: 1 },
    { key: "wednesday", label: "Mercoledì", value: 2 },
    { key: "thursday", label: "Giovedì", value: 3 },
    { key: "friday", label: "Venerdì", value: 4 },
    { key: "saturday", label: "Sabato", value: 5 },
    { key: "sunday", label: "Domenica", value: 6 },
];

// Data (YYYY-MM-DD) del giorno con offset rispetto all'inizio del piano.
function dayDate(startIso, offset) {
    const d = new Date(startIso);
    d.setUTCDate(d.getUTCDate() + offset);
    return d.toISOString().slice(0, 10);
}

function WeeklyPlanDetailPage() {
    const { id } = useParams();
    const planId = Number(id);

    const [plan, setPlan] = useState(null);
    const [foods, setFoods] = useState([]);
    const [usageStats, setUsageStats] = useState([]);
    const [selectedFood, setSelectedFood] = useState({});
    const [symptoms, setSymptoms] = useState([]);
    const [symptomModal, setSymptomModal] = useState({
        open: false,
        dateIso: null,
        initial: null,
    });
    const [symptomToDelete, setSymptomToDelete] = useState(null);
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(true);

    const loadPlan = useCallback(async () => {
        const [planData, statsData] = await Promise.all([
            getWeeklyPlanByDay(planId),
            getWeeklyPlanUsageStats(planId),
        ]);
        setPlan(planData);
        setUsageStats(statsData);
        const from = `${dayDate(planData.startDate, 0)}T00:00:00`;
        const to = `${dayDate(planData.startDate, 6)}T23:59:59`;
        setSymptoms(await getSymptoms(from, to));
    }, [planId]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const foodsData = await getFoodAlternatives();
                setFoods(foodsData);
                await loadPlan();
            } catch (err) {
                console.error(err);
                setError("Impossibile caricare il piano.");
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, [loadPlan]);

    const handleAdd = async (day) => {
        const foodAlternativeId = Number(selectedFood[day.key]);
        if (!foodAlternativeId) {
            setError("Seleziona un alimento da aggiungere.");
            return;
        }
        setError("");
        try {
            await createPlannedMeal({
                dayOfWeek: day.value,
                weeklyPlanId: planId,
                foodAlternativeId,
            });
            setSelectedFood({ ...selectedFood, [day.key]: "" });
            await loadPlan();
        } catch (err) {
            console.error(err);
            setError("Impossibile aggiungere il pasto. Riprova.");
        }
    };

    const handleToggleEaten = async (meal) => {
        setError("");
        try {
            await updatePlannedMeal(meal.id, {
                dayOfWeek: meal.dayOfWeek,
                weeklyPlanId: planId,
                foodAlternativeId: meal.foodAlternativeId,
                eaten: !meal.eaten,
            });
            await loadPlan();
        } catch (err) {
            console.error(err);
            setError("Impossibile aggiornare il pasto. Riprova.");
        }
    };

    const handleRemove = async (mealId) => {
        setError("");
        try {
            await deletePlannedMeal(mealId);
            await loadPlan();
        } catch (err) {
            console.error(err);
            setError("Impossibile rimuovere il pasto. Riprova.");
        }
    };

    const openAddSymptom = (day) => {
        setSymptomModal({
            open: true,
            dateIso: dayDate(plan.startDate, day.value),
            initial: null,
        });
    };

    const openEditSymptom = (symptom) => {
        setSymptomModal({
            open: true,
            dateIso: symptom.occurredAt.slice(0, 10),
            initial: symptom,
        });
    };

    const closeSymptomModal = () =>
        setSymptomModal({ open: false, dateIso: null, initial: null });

    const handleSaveSymptom = async (payload) => {
        setError("");
        try {
            if (symptomModal.initial) {
                await updateSymptom(symptomModal.initial.id, payload);
            } else {
                await createSymptom(payload);
            }
            closeSymptomModal();
            await loadPlan();
        } catch (err) {
            console.error(err);
            setError("Impossibile salvare il sintomo. Riprova.");
        }
    };

    const confirmDeleteSymptom = async () => {
        const id = symptomToDelete.id;
        setSymptomToDelete(null);
        try {
            await deleteSymptom(id);
            await loadPlan();
        } catch (err) {
            console.error(err);
            setError("Impossibile eliminare il sintomo. Riprova.");
        }
    };

    if (loading) {
        return (
            <div className="page">
                <div className="empty-state">Caricamento…</div>
            </div>
        );
    }

    if (!plan) {
        return (
            <div className="page">
                <div className="empty-state">
                    <span className="empty-state-icon">🤔</span>
                    Piano non trovato.{" "}
                    <Link to="/weekly-plans" className="nav-link">
                        Torna ai piani
                    </Link>
                </div>
            </div>
        );
    }

    const overLimit = usageStats.filter((s) => s.isOverLimited);

    return (
        <div className="page">
            <header className="page-header">
                <Link to="/weekly-plans" className="back-link">
                    ← Tutti i piani
                </Link>
                <h1>
                    Settimana dal{" "}
                    {new Date(plan.startDate).toLocaleDateString()} al{" "}
                    {new Date(plan.endDate).toLocaleDateString()}
                </h1>
                <p>
                    Assegna gli alimenti ai giorni, segna cosa hai mangiato e
                    registra i sintomi.
                </p>
            </header>

            {error && <div className="alert alert-error">{error}</div>}

            {overLimit.length > 0 && (
                <div className="alert alert-warning">
                    <strong>Attenzione alle frequenze:</strong>{" "}
                    {overLimit
                        .map(
                            (s) =>
                                `${s.name} (${s.usedCount}/${s.weeklyFrequency})`
                        )
                        .join(", ")}{" "}
                    oltre il limite settimanale.
                </div>
            )}

            <div className="day-grid">
                {DAYS.map((day) => {
                    const meals = plan[day.key] ?? [];
                    const dateIso = dayDate(plan.startDate, day.value);
                    const daySymptoms = symptoms.filter(
                        (s) => s.occurredAt.slice(0, 10) === dateIso
                    );
                    return (
                        <div key={day.key} className="card day-card">
                            <h2 className="day-title">{day.label}</h2>

                            {meals.length === 0 ? (
                                <p className="day-empty">Nessun pasto</p>
                            ) : (
                                <ul className="meal-list">
                                    {meals.map((meal) => (
                                        <li
                                            key={meal.id}
                                            className={
                                                meal.eaten
                                                    ? "meal-item meal-eaten"
                                                    : "meal-item"
                                            }
                                        >
                                            <label
                                                className="meal-check"
                                                title={
                                                    meal.eaten
                                                        ? "Mangiato"
                                                        : "Segna come mangiato"
                                                }
                                            >
                                                <input
                                                    type="checkbox"
                                                    checked={meal.eaten}
                                                    onChange={() =>
                                                        handleToggleEaten(meal)
                                                    }
                                                />
                                            </label>
                                            <div className="meal-info">
                                                <span className="meal-name">
                                                    {meal.foodAlternativeName}
                                                </span>
                                                <span className="meal-meta">
                                                    {meal.mealType} ·{" "}
                                                    {meal.quantity}
                                                </span>
                                            </div>
                                            <button
                                                type="button"
                                                className="btn-icon-danger"
                                                title="Rimuovi"
                                                onClick={() =>
                                                    handleRemove(meal.id)
                                                }
                                            >
                                                ✕
                                            </button>
                                        </li>
                                    ))}
                                </ul>
                            )}

                            <div className="day-add">
                                <select
                                    value={selectedFood[day.key] ?? ""}
                                    onChange={(e) =>
                                        setSelectedFood({
                                            ...selectedFood,
                                            [day.key]: e.target.value,
                                        })
                                    }
                                >
                                    <option value="">Aggiungi alimento…</option>
                                    {foods.map((food) => (
                                        <option key={food.id} value={food.id}>
                                            {food.name} ({food.mealType})
                                        </option>
                                    ))}
                                </select>
                                <button
                                    type="button"
                                    className="btn btn-primary btn-sm"
                                    onClick={() => handleAdd(day)}
                                >
                                    +
                                </button>
                            </div>

                            <div className="day-symptoms">
                                <div className="day-symptoms-head">
                                    <span>Sintomi</span>
                                    <button
                                        type="button"
                                        className="btn-link"
                                        onClick={() => openAddSymptom(day)}
                                    >
                                        + Sintomo
                                    </button>
                                </div>
                                {daySymptoms.length > 0 && (
                                    <ul className="symptom-list">
                                        {daySymptoms.map((s) => (
                                            <li
                                                key={s.id}
                                                className="symptom-item"
                                            >
                                                <span
                                                    className={`sev-dot sev-${s.severity}`}
                                                    title={`Severità ${s.severity}`}
                                                />
                                                <div className="symptom-info">
                                                    <span className="symptom-name">
                                                        {s.type}
                                                    </span>
                                                    <span className="symptom-meta">
                                                        {s.occurredAt.slice(
                                                            11,
                                                            16
                                                        )}
                                                        {s.foodAlternativeName
                                                            ? ` · ${s.foodAlternativeName}`
                                                            : ""}
                                                    </span>
                                                </div>
                                                <button
                                                    type="button"
                                                    className="btn-icon"
                                                    title="Modifica"
                                                    onClick={() =>
                                                        openEditSymptom(s)
                                                    }
                                                >
                                                    ✎
                                                </button>
                                                <button
                                                    type="button"
                                                    className="btn-icon-danger"
                                                    title="Elimina"
                                                    onClick={() =>
                                                        setSymptomToDelete(s)
                                                    }
                                                >
                                                    ✕
                                                </button>
                                            </li>
                                        ))}
                                    </ul>
                                )}
                            </div>
                        </div>
                    );
                })}
            </div>

            {symptomModal.open && (
                <SymptomFormModal
                    key={`${symptomModal.dateIso}-${
                        symptomModal.initial?.id ?? "new"
                    }`}
                    dateIso={symptomModal.dateIso}
                    foods={foods}
                    initial={symptomModal.initial}
                    onSave={handleSaveSymptom}
                    onCancel={closeSymptomModal}
                />
            )}

            <ConfirmDialog
                open={symptomToDelete !== null}
                title="Elimina sintomo"
                message={
                    symptomToDelete
                        ? `Vuoi eliminare il sintomo "${symptomToDelete.type}"? L'azione non è reversibile.`
                        : ""
                }
                confirmLabel="Elimina"
                onConfirm={confirmDeleteSymptom}
                onCancel={() => setSymptomToDelete(null)}
            />
        </div>
    );
}

export default WeeklyPlanDetailPage;
