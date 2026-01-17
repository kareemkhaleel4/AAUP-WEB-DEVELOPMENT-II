const KEY = "linkedinlite_user";

export const authStore = {
  login(data) {
    localStorage.setItem(KEY, JSON.stringify(data));
  },

  logout() {
    localStorage.removeItem(KEY);
  },

  getUser() {
    const raw = localStorage.getItem(KEY);
    return raw ? JSON.parse(raw) : null;
  },

  isAuthenticated() {
    return !!this.getUser()?.token;
  }
};
