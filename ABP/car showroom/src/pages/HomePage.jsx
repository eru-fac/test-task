import { useEffect, useMemo, useState } from 'react';
import { getVehicles } from '../api/vehicles.js';
import VehicleCard from '../components/VehicleCard.jsx';
import Loader from '../components/Loader.jsx';
import EmptyState from '../components/EmptyState.jsx';
import { normalizeSearchText } from '../utils/format.js';

export default function HomePage({ navigate }) {
  const [vehicles, setVehicles] = useState([]);
  const [query, setQuery] = useState('');
  const [sortBy, setSortBy] = useState('rating');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let isActive = true;

    getVehicles().then((data) => {
      if (!isActive) {
        return;
      }

      setVehicles(data);
      setLoading(false);
    });

    return () => {
      isActive = false;
    };
  }, []);

  const filteredVehicles = useMemo(() => {
    const searchValue = normalizeSearchText(query);

    return vehicles
      .filter((vehicle) => {
        const vehicleText = normalizeSearchText(
          `${vehicle.title} ${vehicle.brand} ${vehicle.description}`
        );

        return vehicleText.includes(searchValue);
      })
      .sort((firstVehicle, secondVehicle) => {
        if (sortBy === 'price') {
          return firstVehicle.price - secondVehicle.price;
        }

        if (sortBy === 'name') {
          return firstVehicle.title.localeCompare(secondVehicle.title);
        }

        return secondVehicle.rating - firstVehicle.rating;
      });
  }, [vehicles, query, sortBy]);

  return (
    <>
      <section className="hero-section">
        <div>
          <p className="eyebrow">React test task</p>
          <h1>Simple virtual car showroom</h1>
          <p className="hero-text">
            Browse vehicles, check details and leave your own comments.
          </p>
        </div>
      </section>

      <section className="panel" aria-labelledby="filters-title">
        <h2 id="filters-title">Find a car</h2>

        <form className="filters-form" onSubmit={(event) => event.preventDefault()}>
          <label>
            Search
            <input
              type="search"
              value={query}
              onChange={(event) => setQuery(event.target.value)}
              maxLength="50"
              placeholder="Brand, model or description"
            />
          </label>

          <label>
            Sort by
            <select value={sortBy} onChange={(event) => setSortBy(event.target.value)}>
              <option value="rating">Rating</option>
              <option value="price">Price</option>
              <option value="name">Name</option>
            </select>
          </label>
        </form>
      </section>

      <section className="vehicles-section" aria-labelledby="vehicles-title">
        <div className="section-heading">
          <h2 id="vehicles-title">Available vehicles</h2>
          <span>{filteredVehicles.length} found</span>
        </div>

        {loading ? <Loader text="Loading vehicles..." /> : null}

        {!loading && filteredVehicles.length === 0 ? (
          <EmptyState
            title="No vehicles found"
            text="Try another search request or clear the field."
          />
        ) : null}

        {!loading && filteredVehicles.length > 0 ? (
          <div className="vehicle-grid">
            {filteredVehicles.map((vehicle) => (
              <VehicleCard key={vehicle.id} vehicle={vehicle} navigate={navigate} />
            ))}
          </div>
        ) : null}
      </section>
    </>
  );
}
