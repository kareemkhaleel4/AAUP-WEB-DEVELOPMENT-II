import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { register } from "../services/auth.service";
import { authStore } from "../store/auth.store";

export default function Register() {
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const submit = async (e) => {
    e.preventDefault();
    const res = await register(fullName, email, password);
    authStore.login(res);
    navigate("/feed");
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <h2>Join LinkedInLite</h2>
        <form onSubmit={submit}>
          <input placeholder="Full name" value={fullName} onChange={e => setFullName(e.target.value)} />
          <input placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} />
          <input type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)} />
          <button className="btn btn-primary full">Join now</button>
        </form>
      </div>
    </div>
  );
}
