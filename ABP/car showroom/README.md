# Car Showroom

A small React test task: virtual car showroom with vehicle list, vehicle page and comments.

## Live Demo

Deploy link: `add your Vercel / Netlify link here`


## What the app can do

- shows a list of vehicles on the main page;
- loads vehicle data from DummyJSON API;
- has search by brand, model or description;
- has simple sorting by rating, price or name;
- opens a separate page for each vehicle;
- shows vehicle price, rating, stock and category;
- shows vehicle reviews;
- allows adding a new comment;
- validates comment form fields;
- saves added comments in `localStorage`;
- keeps comments after page reload;
- has responsive layout for mobile and desktop screens.

## API

The app uses DummyJSON products category:

```txt
https://dummyjson.com/products/category/vehicle?limit=24
```

If the API is not available, the app shows a small local fallback list. This is added so the page does not stay empty during API problems.

`

## Tech Stack

- React
- Vite
- JavaScript
- CSS without UI frameworks
- LocalStorage
- DummyJSON API

## Local Start

Install dependencies:

```bash
npm install
```

Start dev server:

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
