import { useState } from "react";
import { likePost, unlikePost } from "../../services/reactions.service";

export default function LikeButton({ postId, initialCount }) {
  const [liked, setLiked] = useState(false);
  const [count, setCount] = useState(initialCount);

  const toggle = async () => {
    if (!liked) {
      const res = await likePost(postId);
      setCount(res.likesCount);
    } else {
      const res = await unlikePost(postId);
      setCount(res.likesCount);
    }
    setLiked(!liked);
  };

  return (
    <button className="btn btn-ghost" onClick={toggle}>
      👍 Like · {count}
    </button>
  );
}
