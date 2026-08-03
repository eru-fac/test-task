import { Link } from 'react-router-dom';

export default function NotFoundPage() {
  return (
    <section className="card not-found">
      <h2>Page not found</h2>
      <p className="muted">This page does not exist.</p>
      <Link className="text-link" to="/">Go home</Link>
    </section>
  );
}
