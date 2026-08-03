const HISTORY_KEY = 'vinDecoderHistory';
const MAX_HISTORY_ITEMS = 3;

export function getVinHistory() {
  try {
    const value = localStorage.getItem(HISTORY_KEY);
    const parsed = JSON.parse(value || '[]');
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
}

export function saveVinToHistory(vin) {
  const normalizedVin = vin.trim().toUpperCase();
  const currentHistory = getVinHistory();
  const nextHistory = [
    normalizedVin,
    ...currentHistory.filter((item) => item !== normalizedVin)
  ].slice(0, MAX_HISTORY_ITEMS);

  localStorage.setItem(HISTORY_KEY, JSON.stringify(nextHistory));
  return nextHistory;
}
