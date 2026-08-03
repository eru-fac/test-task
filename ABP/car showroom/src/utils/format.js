export function formatPrice(price) {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    maximumFractionDigits: 0
  }).format(price || 0);
}

export function normalizeSearchText(value) {
  return String(value || '').trim().toLowerCase();
}
