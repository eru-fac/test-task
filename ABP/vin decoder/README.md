# VIN Decoder

A small React test task for decoding vehicle VIN codes with the public NHTSA API.

The app allows a user to enter a VIN code, validate it, decode it, see only filled vehicle values, reuse the last three requests and browse the list of available NHTSA vehicle variables.

## Live Demo

Deploy link: `add your Vercel / Netlify link here`


## Features

- VIN input form
- Basic VIN validation
- Error messages in the interface
- Decode VIN request to NHTSA API
- Display of decoded vehicle data
- Only filled values are shown
- Last 3 VIN requests history
- Reuse VIN from history
- Vehicle variables page
- Single variable details page
- Responsive layout from mobile to desktop
- No CSS frameworks

## Tech Stack

- React
- Vite
- React Router
- NHTSA vPIC API
- CSS
- localStorage

## API Used

VIN decoding:

```text
https://vpic.nhtsa.dot.gov/api/vehicles/decodevin/{vin}?format=json
```

Vehicle variables:

```text
https://vpic.nhtsa.dot.gov/api/vehicles/getvehiclevariablelist?format=json
```

## Local Start

Install dependencies:

```bash
npm install
```

Run project:

```bash
npm run dev
```

Build project:

```bash
npm run build
```

Preview build:

```bash
npm run preview
```


## Notes

The app uses `localStorage` only for the last three VIN requests. The decoded results are fetched from the API each time.


