export default function Header({ navigate }) {
  return (
    <header className="site-header">
      <nav className="top-navigation" aria-label="Main navigation">
        <button className="logo-button" type="button" onClick={() => navigate('/')}>
          Car Showroom
        </button>

        <button className="nav-button" type="button" onClick={() => navigate('/')}>
          Vehicles
        </button>
      </nav>
    </header>
  );
}
