import { API_BASE_URL } from "../../config";
import { deletePost } from "../../services/posts.service";
import LikeButton from "./LikeButton";
import CommentList from "./CommentList";
import AddComment from "./AddComment";
import { useState } from "react";

export default function PostCard({ post, onChanged }) {
  const [commentsVersion, setCommentsVersion] = useState(0);

  const remove = async () => {
    if (!confirm("Delete this post?")) return;
    await deletePost(post.postId);
    onChanged();
  };

  const refreshComments = () => {
    setCommentsVersion(v => v + 1);
  };

  return (
    <div className="card post">
      <div className="post-header-row">
        <div className="author">
          <img
            className="avatar-sm"
            src={`${API_BASE_URL}${post.authorImageUrl}`}
          />

          <div>
            <strong>{post.authorName}</strong>
            <div className="muted">{post.authorHeadline}</div>
          </div>
        </div>

        {post.isOwner && (
          <button className="btn btn-delete" onClick={remove}>
            Delete
          </button>
        )}
      </div>

      <p>{post.content}</p>

      {post.imageUrls?.length > 0 && (
        <div className="post-images">
          {post.imageUrls.map((url, i) => (
            <img key={i} src={`${API_BASE_URL}${url}`} alt="" />
          ))}
        </div>
      )}

      <LikeButton
        postId={post.postId}
        initialCount={post.likesCount}
      />

      <CommentList
        postId={post.postId}
        refreshKey={commentsVersion}
      />

      <AddComment
        postId={post.postId}
        onAdded={refreshComments}
      />
    </div>
  );
}
