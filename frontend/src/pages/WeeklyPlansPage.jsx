import { useState, useEffect } from "react";
import { getAllWeeklyPlans } from "../services/weeklyPlanService";
import { createWeeklyPlan } from "../services/weeklyPlanService";
function WeeklyPlansPage() {
    const [weeklyPlans, setWeeklyPlans] = useState([]);
    const [form, setForm] = useState({
        startDate: "",
        endDate: "",
    });

    const handleChange = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (form.startDate >= form.endDate) {
            alert("La data di inizio deve essere precedente alla data di fine.");
        }
        try {
            const plan = await createWeeklyPlan(form);
            setWeeklyPlans([...weeklyPlans, plan]);
            setForm({
                startDate: "",
                endDate: "",
            })
        }
        catch (error) {
            console.error(error);
        }
    }

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await getAllWeeklyPlans();
                setWeeklyPlans(data);
            }
            catch (error) {
                console.error
            }
        };
        fetchData();
    }, []);

    return (
        <div>

            <form onSubmit={handleSubmit}>
                <input
                    type="date"
                    name="startDate"
                    placeholder=""
                    value={form.startDate}
                    onChange={handleChange}
                />
                <input
                    type="date"
                    name="endDate"
                    placeholder=""
                    value={form.endDate}
                    onChange={handleChange}
                />
                <button type="submit">SAVE</button>
            </form>
            <h2>Weekly Plans</h2>

            {weeklyPlans.map((plan) => (
                <div key={plan.id}>
                    Settimana dal: <strong>
                        {new Date(plan.startDate).toLocaleDateString()}
                    </strong> al{" "}
                    <strong>{new Date(plan.endDate).toLocaleDateString()}</strong>
                </div>
            ))}
        </div>
    );
}

export default WeeklyPlansPage;