import { useEffect, useState } from 'react';
import VinForm from '../components/VinForm.jsx';
import HistoryList from '../components/HistoryList.jsx';
import ResultTable from '../components/ResultTable.jsx';
import StatusMessage from '../components/StatusMessage.jsx';
import { decodeVin, getFilledVinResults } from '../api/nhtsaApi.js';
import { getVinHistory, saveVinToHistory } from '../utils/historyStorage.js';
import { validateVin } from '../utils/vinValidation.js';

const EXAMPLE_VINS = [
  '1FTFW1CT5DFC10312',
  'JN1AZ4EH7DM430111',
  'WDDGF3BB4DF968608'
];

export default function HomePage() {
  const [vin, setVin] = useState('');
  const [formError, setFormError] = useState('');
  const [apiMessage, setApiMessage] = useState('');
  const [apiError, setApiError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [results, setResults] = useState([]);
  const [history, setHistory] = useState([]);

  useEffect(() => {
    setHistory(getVinHistory());
  }, []);

  async function runDecode(nextVin) {
    const validationError = validateVin(nextVin);

    if (validationError) {
      setFormError(validationError);
      return;
    }

    setIsLoading(true);
    setFormError('');
    setApiError('');
    setApiMessage('');

    try {
      const decodedData = await decodeVin(nextVin);
      const filledResults = getFilledVinResults(decodedData.results);

      setResults(filledResults);
      setApiMessage(decodedData.message);
      setHistory(saveVinToHistory(nextVin));
    } catch (error) {
      setApiError(error.message || 'Something went wrong.');
      setResults([]);
    } finally {
      setIsLoading(false);
    }
  }

  function handleSubmit(event) {
    event.preventDefault();
    runDecode(vin);
  }

  function handleHistorySelect(item) {
    setVin(item);
    runDecode(item);
  }

  function handleExampleClick(item) {
    setVin(item);
    runDecode(item);
  }

  return (
    <>
      <section className="hero-section">
        <div>
          <p className="eyebrow">Vehicle Identification Number</p>
          <h2>Decode car VIN codes</h2>
          <p>
            Enter a VIN code and get vehicle information from the public NHTSA API.
          </p>
        </div>
      </section>

      <div className="layout-grid">
        <div className="main-column">
          <VinForm
            vin={vin}
            error={formError}
            isLoading={isLoading}
            onVinChange={setVin}
            onSubmit={handleSubmit}
          />

          <StatusMessage type="info">{apiMessage}</StatusMessage>
          <StatusMessage type="error">{apiError}</StatusMessage>

          <ResultTable results={results} />
        </div>

        <aside className="side-column">
          <HistoryList history={history} onSelect={handleHistorySelect} />

          <section className="card small-card" aria-labelledby="examples-title">
            <h2 id="examples-title">Example VINs</h2>
            <div className="example-list">
              {EXAMPLE_VINS.map((item) => (
                <button key={item} type="button" onClick={() => handleExampleClick(item)}>
                  {item}
                </button>
              ))}
            </div>
          </section>
        </aside>
      </div>
    </>
  );
}
