import { useEffect, useState } from "react";
import { getComments, deleteComment } from "../../services/comments.service";
import { API_BASE_URL } from "../../config";

export default function CommentList({ postId, refreshKey }) {
  const [comments, setComments] = useState([]);

  const load = async () => {
    const data = await getComments(postId);
    setComments(data);
  };

  useEffect(() => {
    load();
  }, [postId, refreshKey]);

  const remove = async (commentId) => {
    if (!confirm("Delete this comment?")) return;
    await deleteComment(postId, commentId);
    load();
  };

  return (
    <div className="comments">
      {comments.map((c) => (
        <div key={c.commentId} className="comment">
          <div className="comment-row">
            <div className="author">
              <img
                className="avatar-xs"
                src={`${API_BASE_URL}${c.authorImageUrl}`
                }
                alt=""
              />

              <div>
                <strong>{c.authorName}</strong>
                <div className="muted">{c.authorHeadline}</div>
                <div>{c.content}</div>
              </div>
            </div>

            {c.isOwner && (
              <button
                className="btn-delete"
                onClick={() => remove(c.commentId)}
              >
                Delete
              </button>
            )}
          </div>
        </div>
      ))}
    </div>
  );
}
