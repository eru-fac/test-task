export default function VinForm({ vin, error, isLoading, onVinChange, onSubmit }) {
  return (
    <form className="card form-card" onSubmit={onSubmit} noValidate>
      <label htmlFor="vin">VIN code</label>
      <div className="input-row">
        <input
          id="vin"
          type="text"
          value={vin}
          maxLength="17"
          placeholder="Example: 1FTFW1CT5DFC10312"
          onChange={(event) => onVinChange(event.target.value.toUpperCase())}
          aria-describedby={error ? 'vin-error' : undefined}
        />
        <button type="submit" disabled={isLoading}>
          {isLoading ? 'Decoding...' : 'Decode'}
        </button>
      </div>

      {error && (
        <p className="error-text" id="vin-error">
          {error}
        </p>
      )}

      <p className="hint">
        VIN may contain numbers and Latin letters. Letters I, O and Q are not used.
      </p>
    </form>
  );
}
