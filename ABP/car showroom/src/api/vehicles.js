const VEHICLES_API_URL = 'https://dummyjson.com/products/category/vehicle?limit=24';

const fallbackVehicles = [
  {
    id: 101,
    title: 'Family SUV',
    brand: 'Roadline',
    price: 28500,
    rating: 4.7,
    stock: 8,
    category: 'vehicle',
    description: 'Comfortable SUV for city driving and family trips.',
    thumbnail: '',
    images: [],
    reviews: [
      {
        reviewerName: 'Alex Morgan',
        rating: 5,
        comment: 'Comfortable car, good space inside and simple controls.'
      },
      {
        reviewerName: 'Kate Brown',
        rating: 4,
        comment: 'Nice option for everyday use and long weekends.'
      }
    ]
  },
  {
    id: 102,
    title: 'City Hatchback',
    brand: 'UrbanAuto',
    price: 17900,
    rating: 4.4,
    stock: 14,
    category: 'vehicle',
    description: 'Small car for city traffic, easy parking and daily use.',
    thumbnail: '',
    images: [],
    reviews: [
      {
        reviewerName: 'Nick Carter',
        rating: 5,
        comment: 'Easy to park and cheap to maintain.'
      }
    ]
  },
  {
    id: 103,
    title: 'Sport Coupe',
    brand: 'Falcon',
    price: 41500,
    rating: 4.8,
    stock: 4,
    category: 'vehicle',
    description: 'Fast coupe with a clean design and strong performance.',
    thumbnail: '',
    images: [],
    reviews: [
      {
        reviewerName: 'Oliver Smith',
        rating: 5,
        comment: 'Looks great and feels fast on the road.'
      }
    ]
  },
  {
    id: 104,
    title: 'Electric Sedan',
    brand: 'Voltix',
    price: 36200,
    rating: 4.6,
    stock: 7,
    category: 'vehicle',
    description: 'Quiet electric sedan with a modern interior.',
    thumbnail: '',
    images: [],
    reviews: [
      {
        reviewerName: 'Emma Wilson',
        rating: 4,
        comment: 'Quiet, smooth and comfortable for daily driving.'
      }
    ]
  },
  {
    id: 105,
    title: 'Work Pickup',
    brand: 'Northline',
    price: 33100,
    rating: 4.5,
    stock: 6,
    category: 'vehicle',
    description: 'Practical pickup for work tasks and active weekends.',
    thumbnail: '',
    images: [],
    reviews: [
      {
        reviewerName: 'Daniel Green',
        rating: 4,
        comment: 'Good loading space and reliable feeling.'
      }
    ]
  },
  {
    id: 106,
    title: 'Compact Crossover',
    brand: 'MetroCar',
    price: 24600,
    rating: 4.3,
    stock: 11,
    category: 'vehicle',
    description: 'Compact crossover with simple controls and enough space.',
    thumbnail: '',
    images: [],
    reviews: [
      {
        reviewerName: 'Sophia Clark',
        rating: 4,
        comment: 'Good middle option between a hatchback and SUV.'
      }
    ]
  }
];

function normalizeVehicle(product) {
  return {
    id: product.id,
    title: product.title || 'Untitled vehicle',
    brand: product.brand || 'Unknown brand',
    price: Number(product.price) || 0,
    rating: Number(product.rating) || 0,
    stock: Number(product.stock) || 0,
    category: product.category || 'vehicle',
    description: product.description || 'No description provided.',
    thumbnail: product.thumbnail || '',
    images: Array.isArray(product.images) ? product.images : [],
    reviews: Array.isArray(product.reviews) ? product.reviews : []
  };
}

export async function getVehicles() {
  try {
    const response = await fetch(VEHICLES_API_URL);

    if (!response.ok) {
      throw new Error('Vehicle list request failed');
    }

    const data = await response.json();
    const products = Array.isArray(data.products) ? data.products : [];

    if (products.length === 0) {
      return fallbackVehicles.map(normalizeVehicle);
    }

    return products.map(normalizeVehicle);
  } catch (error) {
    console.warn(error);
    return fallbackVehicles.map(normalizeVehicle);
  }
}

export async function getVehicleById(vehicleId) {
  const vehicles = await getVehicles();
  const localVehicle = vehicles.find((vehicle) => String(vehicle.id) === String(vehicleId));

  if (localVehicle) {
    return localVehicle;
  }

  try {
    const response = await fetch(`https://dummyjson.com/products/${vehicleId}`);

    if (!response.ok) {
      throw new Error('Vehicle request failed');
    }

    const data = await response.json();
    return normalizeVehicle(data);
  } catch (error) {
    console.warn(error);
    return null;
  }
}
