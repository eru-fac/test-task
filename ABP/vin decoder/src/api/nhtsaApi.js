const BASE_URL = 'https://vpic.nhtsa.dot.gov/api/vehicles';

async function requestJson(url) {
  const response = await fetch(url);

  if (!response.ok) {
    throw new Error('API request failed. Please try again later.');
  }

  return response.json();
}

export async function decodeVin(vin) {
  const safeVin = encodeURIComponent(vin.trim().toUpperCase());
  const data = await requestJson(`${BASE_URL}/decodevin/${safeVin}?format=json`);

  return {
    message: data.Message || '',
    searchCriteria: data.SearchCriteria || '',
    results: Array.isArray(data.Results) ? data.Results : []
  };
}

export async function getVehicleVariables() {
  const data = await requestJson(`${BASE_URL}/getvehiclevariablelist?format=json`);
  return Array.isArray(data.Results) ? data.Results : [];
}

export function getFilledVinResults(results) {
  return results.filter((item) => {
    const value = String(item.Value || '').trim();
    return value.length > 0 && value.toLowerCase() !== 'not applicable';
  });
}
