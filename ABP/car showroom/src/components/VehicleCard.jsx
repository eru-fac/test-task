import placeholderImage from '../assets/car-placeholder.svg';
import { formatPrice } from '../utils/format.js';

export default function VehicleCard({ vehicle, navigate }) {
  const image = vehicle.thumbnail || vehicle.images?.[0] || placeholderImage;

  return (
    <article className="vehicle-card">
      <img
        src={image}
        alt={vehicle.title}
        onError={(event) => {
          event.currentTarget.src = placeholderImage;
        }}
      />

      <div className="vehicle-card__body">
        <p className="vehicle-brand">{vehicle.brand}</p>
        <h3>{vehicle.title}</h3>
        <p className="vehicle-description">{vehicle.description}</p>

        <div className="vehicle-card__meta" aria-label="Vehicle quick info">
          <span>{formatPrice(vehicle.price)}</span>
          <span>★ {vehicle.rating}</span>
        </div>

        <button type="button" onClick={() => navigate(`/vehicles/${vehicle.id}`)}>
          View details
        </button>
      </div>
    </article>
  );
}
