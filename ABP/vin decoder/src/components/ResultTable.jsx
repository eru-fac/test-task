export default function ResultTable({ results }) {
  if (!results.length) {
    return (
      <section className="card" aria-labelledby="results-title">
        <h2 id="results-title">Decoded results</h2>
        <p className="muted">Enter VIN code to see decoded information.</p>
      </section>
    );
  }

  return (
    <section className="card" aria-labelledby="results-title">
      <div className="section-header">
        <div>
          <h2 id="results-title">Decoded results</h2>
          <p className="muted">Only filled values are shown.</p>
        </div>
        <span className="counter">{results.length} items</span>
      </div>

      <div className="table-wrap">
        <table>
          <thead>
            <tr>
              <th>Variable</th>
              <th>Value</th>
            </tr>
          </thead>
          <tbody>
            {results.map((item) => (
              <tr key={`${item.VariableId}-${item.Variable}`}>
                <td>{item.Variable}</td>
                <td>{item.Value}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </section>
  );
}
