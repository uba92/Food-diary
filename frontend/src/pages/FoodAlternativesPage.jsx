import { useEffect, useState } from "react";
import { getFoodAlternatives, createFoodAlternative } from "../services/foodAlternativeService";

function FoodAlternativesPage() {
    const [foods, setFoods] = useState([]);
    const [form, setForm] = useState({
        name: "",
        mealType: "",
        quantity: "",
        weeklyFrequency: 0,
        notes: ""
    });

    const handleChange = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const createdFood = await createFoodAlternative(form);

            setFoods([...foods, createdFood]);

            setForm({
                name: "",
                mealType: "",
                quantity: "",
                weeklyFrequency: 0,
                notes: ""
            });
        } catch (error) {
            console.error(error);
        }
    };
    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await getFoodAlternatives();
                setFoods(data);
            }
            catch (error) {
                console.error(error);
            }
        };
        fetchData();
    }, []);

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <input
                    name="name"
                    placeholder="Name"
                    value={form.name}
                    onChange={handleChange}
                />

                <input
                    name="mealType"
                    placeholder="Meal type"
                    value={form.mealType}
                    onChange={handleChange}
                />

                <input
                    name="quantity"
                    placeholder="Quantity"
                    value={form.quantity}
                    onChange={handleChange}
                />

                <input
                    name="weeklyFrequency"
                    type="number"
                    placeholder="Weekly frequency"
                    value={form.weeklyFrequency}
                    onChange={handleChange}
                />

                <input
                    name="notes"
                    placeholder="Notes"
                    value={form.notes}
                    onChange={handleChange}
                />

                <button type="submit">Add food</button>
            </form>
            <h2>Food Alternatives</h2>

            {foods.map((food) => (
                <div key={food.id}>
                    <strong>{food.name}</strong> - {food.mealType} - {food.quantity}
                </div>
            ))}
        </div>
    );
}

export default FoodAlternativesPage;