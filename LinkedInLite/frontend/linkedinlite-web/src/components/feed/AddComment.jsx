import { useState } from "react";
import { addComment } from "../../services/comments.service";

export default function AddComment({ postId, onAdded }) {
  const [text, setText] = useState("");

  const submit = async (e) => {
    e.preventDefault();
    if (!text.trim()) return;

    await addComment(postId, text);
    setText("");
    onAdded();
  };

  return (
    <form onSubmit={submit} className="comment-box">
      <input
        placeholder="Add a comment..."
        value={text}
        onChange={(e) => setText(e.target.value)}
      />
    </form>
  );
}
