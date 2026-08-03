const VIN_ALLOWED_PATTERN = /^[A-HJ-NPR-Z0-9]+$/i;

export function validateVin(value) {
  const vin = value.trim();

  if (!vin) {
    return 'Enter VIN code.';
  }

  if (vin.length > 17) {
    return 'VIN must be no longer than 17 characters.';
  }

  if (!VIN_ALLOWED_PATTERN.test(vin)) {
    return 'VIN can contain only numbers and Latin letters except I, O and Q.';
  }

  return '';
}
