export default function HistoryList({ history, onSelect }) {
  if (!history.length) {
    return (
      <section className="card small-card" aria-labelledby="history-title">
        <h2 id="history-title">Recent VIN codes</h2>
        <p className="muted">No recent requests yet.</p>
      </section>
    );
  }

  return (
    <section className="card small-card" aria-labelledby="history-title">
      <h2 id="history-title">Recent VIN codes</h2>
      <ul className="history-list">
        {history.map((item) => (
          <li key={item}>
            <button type="button" onClick={() => onSelect(item)}>
              {item}
            </button>
          </li>
        ))}
      </ul>
    </section>
  );
}
