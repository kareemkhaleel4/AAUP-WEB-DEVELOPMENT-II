import api from "./api";

export const getComments = async (postId) => {
  const res = await api.get(`/posts/${postId}/comments`);
  return res.data;
};

export const addComment = async (postId, content) => {
  await api.post(`/posts/${postId}/comments`, { content });
};

export const deleteComment = async (postId, commentId) => {
  await api.delete(`/posts/${postId}/comments/${commentId}`);
};
