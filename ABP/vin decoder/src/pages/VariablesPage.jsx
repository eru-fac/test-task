import { useEffect, useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import { getVehicleVariables } from '../api/nhtsaApi.js';
import { stripHtml } from '../utils/stripHtml.js';

export default function VariablesPage() {
  const [variables, setVariables] = useState([]);
  const [search, setSearch] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    let ignore = false;

    async function loadVariables() {
      try {
        const data = await getVehicleVariables();
        if (!ignore) {
          setVariables(data);
        }
      } catch (loadError) {
        if (!ignore) {
          setError(loadError.message || 'Could not load variables.');
        }
      } finally {
        if (!ignore) {
          setIsLoading(false);
        }
      }
    }

    loadVariables();

    return () => {
      ignore = true;
    };
  }, []);

  const filteredVariables = useMemo(() => {
    const value = search.trim().toLowerCase();

    if (!value) {
      return variables;
    }

    return variables.filter((item) => {
      const name = String(item.Name || '').toLowerCase();
      const description = stripHtml(item.Description).toLowerCase();
      return name.includes(value) || description.includes(value);
    });
  }, [variables, search]);

  return (
    <section className="card" aria-labelledby="variables-title">
      <div className="section-header">
        <div>
          <h2 id="variables-title">Vehicle variables</h2>
          <p className="muted">List of available variables from NHTSA API.</p>
        </div>
        <span className="counter">{filteredVariables.length}</span>
      </div>

      <label htmlFor="variable-search" className="search-label">
        Search variable
      </label>
      <input
        id="variable-search"
        className="search-input"
        type="search"
        value={search}
        placeholder="Example: Make, Model, Engine"
        onChange={(event) => setSearch(event.target.value)}
      />

      {isLoading && <p className="muted">Loading variables...</p>}
      {error && <p className="status-message error">{error}</p>}

      {!isLoading && !error && (
        <ul className="variable-list">
          {filteredVariables.map((item) => (
            <li key={item.ID}>
              <Link to={`/variables/${item.ID}`}>
                <strong>{item.Name || `Variable ${item.ID}`}</strong>
                <span>{stripHtml(item.Description) || 'No description'}</span>
              </Link>
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
