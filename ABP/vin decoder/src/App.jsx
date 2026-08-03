import { NavLink, Outlet } from 'react-router-dom';

export default function App() {
  return (
    <div className="app-shell">
      <header className="site-header">
        <div>
          <p className="eyebrow">NHTSA API</p>
          <h1>VIN Decoder</h1>
        </div>

        <nav className="main-nav" aria-label="Main navigation">
          <NavLink to="/">Decoder</NavLink>
          <NavLink to="/variables">Variables</NavLink>
        </nav>
      </header>

      <main>
        <Outlet />
      </main>
    </div>
  );
}
