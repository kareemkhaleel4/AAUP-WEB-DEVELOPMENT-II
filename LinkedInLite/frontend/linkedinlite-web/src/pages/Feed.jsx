import { useEffect, useState } from "react";
import { getFeed } from "../services/posts.service";
import CreatePostBox from "../components/feed/CreatePostBox";
import PostCard from "../components/feed/PostCard";

export default function Feed() {
  const [posts, setPosts] = useState([]);

  const load = async () => {
    const data = await getFeed();
    setPosts(data);
  };

  useEffect(() => {
    load();
  }, []);

  return (
    <div className="feed">
      <CreatePostBox onCreated={load} />
      {posts.map(p => (
        <PostCard
          key={p.postId}
          post={p}
          onChanged={load}
        />
      ))}
    </div>
  );
}
