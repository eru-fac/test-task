import { useState } from 'react';

export default function CommentForm({ onAddComment }) {
  const [name, setName] = useState('');
  const [text, setText] = useState('');
  const [error, setError] = useState('');

  function handleSubmit(event) {
    event.preventDefault();

    const cleanName = name.trim();
    const cleanText = text.trim();

    if (!cleanName || !cleanText) {
      setError('Please fill in all fields.');
      return;
    }

    if (cleanName.length > 40) {
      setError('Name must be 40 characters or less.');
      return;
    }

    if (cleanText.length > 300) {
      setError('Comment must be 300 characters or less.');
      return;
    }

    onAddComment({
      reviewerName: cleanName,
      comment: cleanText,
      rating: 5,
      createdAt: new Date().toISOString()
    });

    setName('');
    setText('');
    setError('');
  }

  return (
    <form className="comment-form" onSubmit={handleSubmit} noValidate>
      <h2>Add comment</h2>

      <label>
        Your name
        <input
          value={name}
          onChange={(event) => setName(event.target.value)}
          maxLength="40"
          placeholder="Name"
        />
      </label>

      <label>
        Comment
        <textarea
          value={text}
          onChange={(event) => setText(event.target.value)}
          maxLength="300"
          placeholder="Write a short comment"
        />
      </label>

      {error ? <p className="form-error">{error}</p> : null}

      <button type="submit">Add comment</button>
    </form>
  );
}
