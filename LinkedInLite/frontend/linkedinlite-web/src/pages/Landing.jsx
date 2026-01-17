import { Link } from "react-router-dom";

export default function Landing() {
  return (
    <div className="landing">
      <div className="landing-hero">
        <div>
          <h1>Welcome to LinkedInLite</h1>
          <p className="muted">
            A simple professional network where you can share posts, react, and comment.
          </p>

          <div className="landing-actions">
            <Link to="/register" className="btn btn-primary">
              Join now
            </Link>
            <Link to="/login" className="btn btn-ghost">
              Sign in
            </Link>
          </div>

          <div className="landing-note muted">
            Build step-by-step: Auth → Profile → Feed → Post with image → Like → Comment.
          </div>
        </div>

        <div className="landing-card">
          <div className="card-title">What you’ll do here</div>
          <ul className="card-list">
            <li>Create your professional profile</li>
            <li>Publish posts (with optional images)</li>
            <li>Like and comment on posts</li>
            <li>View a timeline feed</li>
          </ul>
        </div>
      </div>
    </div>
  );
}
