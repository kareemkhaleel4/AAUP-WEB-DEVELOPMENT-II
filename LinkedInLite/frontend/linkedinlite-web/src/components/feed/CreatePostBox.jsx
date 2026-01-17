import { useState } from "react";
import { createPost } from "../../services/posts.service";

export default function CreatePostBox({ onCreated }) {
  const [content, setContent] = useState("");
  const [files, setFiles] = useState([]);

  const submit = async (e) => {
    e.preventDefault();
    await createPost(content, files);
    setContent("");
    setFiles([]);
    onCreated();
  };

  return (
    <div className="card create-post">
      <form onSubmit={submit}>
        <textarea
          value={content}
          onChange={e => setContent(e.target.value)}
        />
        <input
          type="file"
          multiple
          onChange={e => setFiles([...e.target.files])}
        />
        <button className="btn btn-primary">Post</button>
      </form>
    </div>
  );
}
