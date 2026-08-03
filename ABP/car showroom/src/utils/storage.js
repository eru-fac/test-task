const COMMENTS_KEY = 'car-showroom-comments';

export function getSavedComments(vehicleId) {
  try {
    const rawValue = localStorage.getItem(COMMENTS_KEY);
    const commentsByVehicle = rawValue ? JSON.parse(rawValue) : {};

    return commentsByVehicle[vehicleId] || [];
  } catch (error) {
    console.warn(error);
    return [];
  }
}

export function saveComment(vehicleId, comment) {
  try {
    const rawValue = localStorage.getItem(COMMENTS_KEY);
    const commentsByVehicle = rawValue ? JSON.parse(rawValue) : {};
    const vehicleComments = commentsByVehicle[vehicleId] || [];

    commentsByVehicle[vehicleId] = [comment, ...vehicleComments];
    localStorage.setItem(COMMENTS_KEY, JSON.stringify(commentsByVehicle));

    return commentsByVehicle[vehicleId];
  } catch (error) {
    console.warn(error);
    return [];
  }
}
