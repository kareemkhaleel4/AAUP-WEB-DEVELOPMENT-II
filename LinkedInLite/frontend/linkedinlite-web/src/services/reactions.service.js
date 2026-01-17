import api from "./api";

export const likePost = async (postId) => {
  const res = await api.post(`/posts/${postId}/like`);
  return res.data; // { likesCount }
};

export const unlikePost = async (postId) => {
  const res = await api.delete(`/posts/${postId}/like`);
  return res.data; // { likesCount }
};
