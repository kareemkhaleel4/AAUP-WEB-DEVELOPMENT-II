import api from "./api";

export const getFeed = async () => {
  const res = await api.get("/posts");
  return res.data;
};

export const createPost = async (content, images) => {
  const form = new FormData();
  form.append("content", content);
  images.forEach(img => form.append("images", img));
  await api.post("/posts", form);
};

export const deletePost = async (postId) => {
  await api.delete(`/posts/${postId}`);
};
