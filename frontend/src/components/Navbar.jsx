import { NavLink } from "react-router-dom";

function Navbar() {
    return (
        <nav className="navbar">
            <div className="navbar-inner">
                <NavLink to="/" className="brand">
                    <span className="brand-mark">🥗</span>
                    <span>FoodDiary</span>
                </NavLink>
                <div className="nav-links">
                    <NavLink
                        to="/weekly-plans"
                        className={({ isActive }) =>
                            isActive ? "nav-link active" : "nav-link"
                        }
                    >
                        Piani settimanali
                    </NavLink>
                    <NavLink
                        to="/food-alternatives"
                        className={({ isActive }) =>
                            isActive ? "nav-link active" : "nav-link"
                        }
                    >
                        Alimenti
                    </NavLink>
                    <NavLink
                        to="/report"
                        className={({ isActive }) =>
                            isActive ? "nav-link active" : "nav-link"
                        }
                    >
                        Report
                    </NavLink>
                </div>
            </div>
        </nav>
    );
}

export default Navbar;
