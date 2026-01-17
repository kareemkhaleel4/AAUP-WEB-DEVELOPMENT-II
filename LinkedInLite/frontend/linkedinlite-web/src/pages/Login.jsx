import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../services/auth.service";
import { authStore } from "../store/auth.store";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const submit = async (e) => {
    e.preventDefault();
    const res = await login(email, password);
    authStore.login(res);
    navigate("/feed");
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <h2>Sign in</h2>
        <form onSubmit={submit}>
          <input placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} />
          <input type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} />
          <button className="btn btn-primary full">Sign in</button>
        </form>
      </div>
    </div>
  );
}
