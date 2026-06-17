import { useState } from "react";

const SYMPTOM_TYPES = [
    "Gonfiore",
    "Nausea",
    "Mal di testa",
    "Crampi",
    "Diarrea",
    "Stitichezza",
    "Reflusso",
    "Stanchezza",
    "Eruzione cutanea",
];
const MEAL_TYPES = ["Colazione", "Spuntino", "Pranzo", "Merenda", "Cena"];
const SEVERITIES = [1, 2, 3, 4, 5];

// Montato solo quando aperto (con key dal parent): lo stato iniziale deriva
// direttamente dalle props, niente useEffect di sincronizzazione.
function SymptomFormModal({ dateIso, foods, initial, onSave, onCancel }) {
    const known = initial ? SYMPTOM_TYPES.includes(initial.type) : false;
    const [time, setTime] = useState(
        initial ? initial.occurredAt.slice(11, 16) : "12:00"
    );
    const [typeChoice, setTypeChoice] = useState(
        initial ? (known ? initial.type : "Altro") : ""
    );
    const [customType, setCustomType] = useState(
        initial && !known ? initial.type : ""
    );
    const [severity, setSeverity] = useState(initial ? initial.severity : 3);
    const [mealType, setMealType] = useState(initial?.mealType ?? "");
    const [foodId, setFoodId] = useState(initial?.foodAlternativeId ?? "");
    const [notes, setNotes] = useState(initial?.notes ?? "");
    const [error, setError] = useState("");

    const submit = () => {
        const type = typeChoice === "Altro" ? customType.trim() : typeChoice;
        if (!type) {
            setError("Indica il tipo di sintomo.");
            return;
        }
        onSave({
            occurredAt: `${dateIso}T${time}:00`,
            type,
            severity: Number(severity),
            mealType: mealType || null,
            foodAlternativeId: foodId ? Number(foodId) : null,
            notes: notes.trim() || null,
        });
    };

    return (
        <div className="modal-overlay" onClick={onCancel}>
            <div
                className="modal modal-wide"
                role="dialog"
                aria-modal="true"
                onClick={(e) => e.stopPropagation()}
            >
                <h3 className="modal-title">
                    {initial ? "Modifica sintomo" : "Nuovo sintomo"}
                </h3>

                <div className="modal-form">
                    <div className="field">
                        <label htmlFor="sym-type">Tipo</label>
                        <select
                            id="sym-type"
                            value={typeChoice}
                            onChange={(e) => setTypeChoice(e.target.value)}
                        >
                            <option value="">Seleziona…</option>
                            {SYMPTOM_TYPES.map((t) => (
                                <option key={t} value={t}>
                                    {t}
                                </option>
                            ))}
                            <option value="Altro">Altro…</option>
                        </select>
                    </div>

                    {typeChoice === "Altro" && (
                        <div className="field">
                            <label htmlFor="sym-custom">Specifica</label>
                            <input
                                id="sym-custom"
                                value={customType}
                                placeholder="Descrivi il sintomo"
                                onChange={(e) => setCustomType(e.target.value)}
                            />
                        </div>
                    )}

                    <div className="field">
                        <label htmlFor="sym-severity">Severità (1–5)</label>
                        <select
                            id="sym-severity"
                            value={severity}
                            onChange={(e) => setSeverity(e.target.value)}
                        >
                            {SEVERITIES.map((s) => (
                                <option key={s} value={s}>
                                    {s}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className="field">
                        <label htmlFor="sym-time">Ora</label>
                        <input
                            id="sym-time"
                            type="time"
                            value={time}
                            onChange={(e) => setTime(e.target.value)}
                        />
                    </div>

                    <div className="field">
                        <label htmlFor="sym-meal">Momento</label>
                        <select
                            id="sym-meal"
                            value={mealType}
                            onChange={(e) => setMealType(e.target.value)}
                        >
                            <option value="">Non specificato</option>
                            {MEAL_TYPES.map((m) => (
                                <option key={m} value={m}>
                                    {m}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className="field">
                        <label htmlFor="sym-food">Alimento sospetto</label>
                        <select
                            id="sym-food"
                            value={foodId}
                            onChange={(e) => setFoodId(e.target.value)}
                        >
                            <option value="">Nessuno</option>
                            {foods.map((f) => (
                                <option key={f.id} value={f.id}>
                                    {f.name}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className="field field-full">
                        <label htmlFor="sym-notes">Note</label>
                        <input
                            id="sym-notes"
                            value={notes}
                            placeholder="Facoltative"
                            onChange={(e) => setNotes(e.target.value)}
                        />
                    </div>
                </div>

                {error && <div className="alert alert-error">{error}</div>}

                <div className="modal-actions">
                    <button
                        type="button"
                        className="btn btn-ghost"
                        onClick={onCancel}
                    >
                        Annulla
                    </button>
                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={submit}
                    >
                        Salva
                    </button>
                </div>
            </div>
        </div>
    );
}

export default SymptomFormModal;
