import { useEffect, useState } from "react";
import {
    getFoodAlternatives,
    createFoodAlternative,
    updateFoodAlternative,
    deleteFoodAlternative,
} from "../services/foodAlternativeService";
import ConfirmDialog from "../components/ConfirmDialog";

const MEAL_TYPES = ["Colazione", "Spuntino", "Pranzo", "Merenda", "Cena"];
const CATEGORIES = [
    "Proteine",
    "Cereali",
    "Verdura",
    "Frutta",
    "Latticini",
    "Grassi",
    "Altro",
];

const emptyForm = {
    name: "",
    mealType: "",
    quantity: "",
    weeklyFrequency: 1,
    notes: "",
    foodCategory: "",
};

// Raggruppa gli alimenti per la chiave scelta, rispettando l'ordine
// predefinito e mettendo eventuali valori extra dopo, con il bucket
// "non assegnato" sempre in fondo.
function groupFoods(foods, key, order, fallbackLabel) {
    const groups = {};
    for (const food of foods) {
        const value = food[key]?.trim();
        const label = value || fallbackLabel;
        (groups[label] ??= []).push(food);
    }
    const labels = Object.keys(groups);
    const ordered = order.filter((o) => groups[o]);
    const extras = labels
        .filter((l) => !order.includes(l) && l !== fallbackLabel)
        .sort();
    const tail = groups[fallbackLabel] ? [fallbackLabel] : [];
    return [...ordered, ...extras, ...tail].map((label) => ({
        label,
        items: groups[label],
    }));
}

function FoodAlternativesPage() {
    const [foods, setFoods] = useState([]);
    const [form, setForm] = useState(emptyForm);
    const [editingId, setEditingId] = useState(null);
    const [toDelete, setToDelete] = useState(null);
    const [error, setError] = useState("");
    const [groupBy, setGroupBy] = useState("mealType");

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

    const startEdit = (food) => {
        setEditingId(food.id);
        setForm({
            name: food.name,
            mealType: food.mealType,
            quantity: food.quantity,
            weeklyFrequency: food.weeklyFrequency,
            notes: food.notes ?? "",
            foodCategory: food.foodCategory ?? "",
        });
        setError("");
        window.scrollTo({ top: 0, behavior: "smooth" });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");

        if (!form.name.trim() || !form.mealType || !form.quantity.trim()) {
            setError("Nome, tipo di pasto e quantità sono obbligatori.");
            return;
        }

        const payload = {
            ...form,
            weeklyFrequency: Number(form.weeklyFrequency),
            foodCategory: form.foodCategory || null,
        };

        try {
            if (editingId) {
                const updated = await updateFoodAlternative(editingId, payload);
                setFoods(foods.map((f) => (f.id === editingId ? updated : f)));
            } else {
                const created = await createFoodAlternative(payload);
                setFoods([...foods, created]);
            }
            resetForm();
        } catch (err) {
            console.error(err);
            setError("Impossibile salvare l'alimento. Riprova.");
        }
    };

    const confirmDelete = async () => {
        const id = toDelete.id;
        setToDelete(null);
        try {
            await deleteFoodAlternative(id);
            setFoods(foods.filter((food) => food.id !== id));
            if (editingId === id) resetForm();
        } catch (err) {
            console.error(err);
            setError("Impossibile eliminare l'alimento. Riprova.");
        }
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await getFoodAlternatives();
                setFoods(data);
            } catch (err) {
                console.error(err);
                setError("Impossibile caricare gli alimenti.");
            }
        };
        fetchData();
    }, []);

    const sections =
        groupBy === "mealType"
            ? groupFoods(foods, "mealType", MEAL_TYPES, "Senza tipo")
            : groupFoods(foods, "foodCategory", CATEGORIES, "Senza categoria");

    return (
        <div className="page">
            <header className="page-header">
                <h1>Alimenti</h1>
                <p>
                    Crea le tue alternative di pasto da riutilizzare nei piani
                    settimanali, con la frequenza consigliata.
                </p>
            </header>

            <section className="card">
                <h2 className="card-title">
                    {editingId ? "Modifica alimento" : "Nuovo alimento"}
                </h2>
                <form onSubmit={handleSubmit}>
                    <div className="form-grid">
                        <div className="field">
                            <label htmlFor="name">Nome</label>
                            <input
                                id="name"
                                name="name"
                                placeholder="Es. Yogurt greco"
                                value={form.name}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="field">
                            <label htmlFor="mealType">Tipo di pasto</label>
                            <select
                                id="mealType"
                                name="mealType"
                                value={form.mealType}
                                onChange={handleChange}
                            >
                                <option value="">Seleziona…</option>
                                {MEAL_TYPES.map((type) => (
                                    <option key={type} value={type}>
                                        {type}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className="field">
                            <label htmlFor="foodCategory">Categoria</label>
                            <select
                                id="foodCategory"
                                name="foodCategory"
                                value={form.foodCategory}
                                onChange={handleChange}
                            >
                                <option value="">Nessuna</option>
                                {CATEGORIES.map((cat) => (
                                    <option key={cat} value={cat}>
                                        {cat}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className="field">
                            <label htmlFor="quantity">Quantità</label>
                            <input
                                id="quantity"
                                name="quantity"
                                placeholder="Es. 150 g"
                                value={form.quantity}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="field">
                            <label htmlFor="weeklyFrequency">
                                Frequenza settimanale
                            </label>
                            <input
                                id="weeklyFrequency"
                                name="weeklyFrequency"
                                type="number"
                                min="0"
                                value={form.weeklyFrequency}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="field">
                            <label htmlFor="notes">Note</label>
                            <input
                                id="notes"
                                name="notes"
                                placeholder="Facoltative"
                                value={form.notes}
                                onChange={handleChange}
                            />
                        </div>

                        <div className="form-actions">
                            <button type="submit" className="btn btn-primary">
                                {editingId ? "Salva modifiche" : "Aggiungi alimento"}
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

            {foods.length === 0 ? (
                <div className="empty-state">
                    <span className="empty-state-icon">🥑</span>
                    Nessun alimento ancora. Aggiungine uno per iniziare.
                </div>
            ) : (
                <>
                    <div className="tabs">
                        <button
                            type="button"
                            className={
                                groupBy === "mealType" ? "tab active" : "tab"
                            }
                            onClick={() => setGroupBy("mealType")}
                        >
                            Per tipo di pasto
                        </button>
                        <button
                            type="button"
                            className={
                                groupBy === "category" ? "tab active" : "tab"
                            }
                            onClick={() => setGroupBy("category")}
                        >
                            Per categoria
                        </button>
                    </div>

                    {sections.map((section) => (
                        <div key={section.label} className="group">
                            <h2 className="group-title">
                                {section.label}
                                <span className="group-count">
                                    {section.items.length}
                                </span>
                            </h2>
                            <div className="list">
                                {section.items.map((food) => (
                                    <div key={food.id} className="list-item">
                                        <div className="item-body">
                                            <div className="item-title">
                                                {food.name}
                                            </div>
                                            <div className="item-sub">
                                                {food.mealType} · {food.quantity}
                                            </div>
                                            {food.notes && (
                                                <div className="item-notes">
                                                    {food.notes}
                                                </div>
                                            )}
                                        </div>
                                        {food.foodCategory && (
                                            <span className="chip chip-accent">
                                                {food.foodCategory}
                                            </span>
                                        )}
                                        <span className="chip">
                                            {food.weeklyFrequency}× / sett.
                                        </span>
                                        <div className="item-actions">
                                            <button
                                                type="button"
                                                className="btn-icon"
                                                title="Modifica"
                                                onClick={() => startEdit(food)}
                                            >
                                                ✎
                                            </button>
                                            <button
                                                type="button"
                                                className="btn-icon-danger"
                                                title="Elimina"
                                                onClick={() => setToDelete(food)}
                                            >
                                                ✕
                                            </button>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </div>
                    ))}
                </>
            )}

            <ConfirmDialog
                open={toDelete !== null}
                title="Elimina alimento"
                message={
                    toDelete
                        ? `Vuoi eliminare "${toDelete.name}"? L'azione non è reversibile.`
                        : ""
                }
                confirmLabel="Elimina"
                onConfirm={confirmDelete}
                onCancel={() => setToDelete(null)}
            />
        </div>
    );
}

export default FoodAlternativesPage;
