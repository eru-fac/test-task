import { useEffect, useState } from 'react';
import Header from './components/Header.jsx';
import Footer from './components/Footer.jsx';
import HomePage from './pages/HomePage.jsx';
import VehiclePage from './pages/VehiclePage.jsx';

function useSimpleRoute() {
  const [path, setPath] = useState(window.location.pathname);

  useEffect(() => {
    const handlePopState = () => setPath(window.location.pathname);

    window.addEventListener('popstate', handlePopState);
    return () => window.removeEventListener('popstate', handlePopState);
  }, []);

  function navigate(nextPath) {
    window.history.pushState({}, '', nextPath);
    setPath(nextPath);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  return { path, navigate };
}

export default function App() {
  const { path, navigate } = useSimpleRoute();
  const vehiclePageMatch = path.match(/^\/vehicles\/(\d+)/);

  return (
    <div className="app-shell">
      <Header navigate={navigate} />

      <main className="main-content">
        {vehiclePageMatch ? (
          <VehiclePage vehicleId={vehiclePageMatch[1]} navigate={navigate} />
        ) : (
          <HomePage navigate={navigate} />
        )}
      </main>

      <Footer />
    </div>
  );
}
