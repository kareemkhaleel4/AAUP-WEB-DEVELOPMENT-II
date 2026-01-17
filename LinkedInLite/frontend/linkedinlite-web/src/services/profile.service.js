import api from "./api";

export const getMyProfile = async () => {
  const res = await api.get("/profile/me");
  return res.data;
};

export const updateProfile = async (data) => {
  await api.put("/profile/me", data);
};

export const uploadProfileImage = async (file) => {
  const form = new FormData();
  form.append("image", file);

  const res = await api.post("/profile/me/image", form);
  return res.data; // image URL
};
