import { useEffect, useState } from 'react';
import { getVehicleById } from '../api/vehicles.js';
import { getSavedComments, saveComment } from '../utils/storage.js';
import { formatPrice } from '../utils/format.js';
import CommentForm from '../components/CommentForm.jsx';
import Loader from '../components/Loader.jsx';
import EmptyState from '../components/EmptyState.jsx';
import placeholderImage from '../assets/car-placeholder.svg';

export default function VehiclePage({ vehicleId, navigate }) {
  const [vehicle, setVehicle] = useState(null);
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let isActive = true;

    setLoading(true);

    getVehicleById(vehicleId).then((data) => {
      if (!isActive) {
        return;
      }

      setVehicle(data);
      setComments(getSavedComments(vehicleId));
      setLoading(false);
    });

    return () => {
      isActive = false;
    };
  }, [vehicleId]);

  function handleAddComment(comment) {
    const updatedComments = saveComment(vehicleId, comment);
    setComments(updatedComments);
  }

  if (loading) {
    return <Loader text="Loading vehicle..." />;
  }

  if (!vehicle) {
    return (
      <EmptyState
        title="Vehicle not found"
        text="This vehicle is not available right now."
        action={
          <button type="button" onClick={() => navigate('/')}>
            Back to showroom
          </button>
        }
      />
    );
  }

  const image = vehicle.thumbnail || vehicle.images?.[0] || placeholderImage;
  const allComments = [...comments, ...(vehicle.reviews || [])];

  return (
    <>
      <button className="back-button" type="button" onClick={() => navigate('/')}>
        ← Back to showroom
      </button>

      <article className="vehicle-details">
        <img
          src={image}
          alt={vehicle.title}
          onError={(event) => {
            event.currentTarget.src = placeholderImage;
          }}
        />

        <div className="vehicle-details__content">
          <p className="vehicle-brand">{vehicle.brand}</p>
          <h1>{vehicle.title}</h1>
          <p>{vehicle.description}</p>

          <dl className="vehicle-specs">
            <div>
              <dt>Price</dt>
              <dd>{formatPrice(vehicle.price)}</dd>
            </div>
            <div>
              <dt>Rating</dt>
              <dd>★ {vehicle.rating}</dd>
            </div>
            <div>
              <dt>Stock</dt>
              <dd>{vehicle.stock}</dd>
            </div>
            <div>
              <dt>Category</dt>
              <dd>{vehicle.category}</dd>
            </div>
          </dl>
        </div>
      </article>

      <section className="comments-layout" aria-labelledby="comments-title">
        <div>
          <h2 id="comments-title">Comments</h2>

          <div className="comments-list">
            {allComments.length === 0 ? (
              <p className="status-message">There are no comments yet.</p>
            ) : (
              allComments.map((comment, index) => (
                <article className="comment-card" key={`${comment.reviewerName}-${index}`}>
                  <div className="comment-card__top">
                    <strong>{comment.reviewerName || comment.name || 'User'}</strong>
                    {comment.rating ? <span>★ {comment.rating}</span> : null}
                  </div>
                  <p>{comment.comment || comment.text}</p>
                </article>
              ))
            )}
          </div>
        </div>

        <CommentForm onAddComment={handleAddComment} />
      </section>
    </>
  );
}
