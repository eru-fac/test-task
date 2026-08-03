import { useEffect, useMemo, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { getVehicleVariables } from '../api/nhtsaApi.js';
import { stripHtml } from '../utils/stripHtml.js';

export default function VariableDetailsPage() {
  const { variableId } = useParams();
  const [variables, setVariables] = useState([]);
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
          setError(loadError.message || 'Could not load variable.');
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

  const variable = useMemo(() => {
    return variables.find((item) => String(item.ID) === String(variableId));
  }, [variables, variableId]);

  if (isLoading) {
    return (
      <section className="card">
        <p className="muted">Loading variable...</p>
      </section>
    );
  }

  if (error) {
    return (
      <section className="card">
        <p className="status-message error">{error}</p>
        <Link className="text-link" to="/variables">Back to variables</Link>
      </section>
    );
  }

  if (!variable) {
    return (
      <section className="card">
        <h2>Variable not found</h2>
        <p className="muted">There is no variable with this id.</p>
        <Link className="text-link" to="/variables">Back to variables</Link>
      </section>
    );
  }

  return (
    <section className="card variable-details" aria-labelledby="variable-title">
      <Link className="text-link" to="/variables">Back to variables</Link>
      <p className="eyebrow">Variable #{variable.ID}</p>
      <h2 id="variable-title">{variable.Name || `Variable ${variable.ID}`}</h2>
      <p>{stripHtml(variable.Description) || 'No description available.'}</p>
    </section>
  );
}
