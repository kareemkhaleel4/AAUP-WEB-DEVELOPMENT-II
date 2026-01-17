import { useEffect, useState } from "react";
import {
  getMyProfile,
  updateProfile,
  uploadProfileImage
} from "../services/profile.service";
import { API_BASE_URL } from "../config";

export default function Profile() {
  const [profile, setProfile] = useState(null);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    load();
  }, []);

  const load = async () => {
    const data = await getMyProfile();
    setProfile(data);
  };

  const save = async () => {
    setSaving(true);
    await updateProfile(profile);
    setSaving(false);
  };

  const uploadImage = async (file) => {
    const url = await uploadProfileImage(file);
    setProfile({ ...profile, profileImageUrl: url });
  };

  if (!profile) return null;

  return (
    <div className="profile-page">
      {/* Header */}
      <div className="profile-card">
        <div className="profile-header">
          <div className="profile-avatar-wrapper">
            <img
              className="profile-avatar"
              src={
                profile.profileImageUrl
                  ? `${API_BASE_URL}${profile.profileImageUrl}`
                  : "/avatar-placeholder.png"
              }
              alt=""
            />

            <label className="avatar-upload">
              Change
              <input
                type="file"
                hidden
                onChange={(e) => uploadImage(e.target.files[0])}
              />
            </label>
          </div>

          <div className="profile-main">
            <input
              className="profile-name"
              value={profile.fullName}
              onChange={(e) =>
                setProfile({ ...profile, fullName: e.target.value })
              }
            />

            <input
              className="profile-headline"
              value={profile.headline}
              onChange={(e) =>
                setProfile({ ...profile, headline: e.target.value })
              }
            />

            <input
              className="profile-location"
              value={profile.location}
              onChange={(e) =>
                setProfile({ ...profile, location: e.target.value })
              }
            />
          </div>
        </div>
      </div>

      {/* Summary */}
      <div className="profile-card">
        <h3>About</h3>
        <textarea
          className="profile-summary"
          value={profile.summary}
          onChange={(e) =>
            setProfile({ ...profile, summary: e.target.value })
          }
          placeholder="Write a short professional summary..."
        />
      </div>

      {/* Actions */}
      <div className="profile-actions">
        <button
          className="btn btn-primary"
          onClick={save}
          disabled={saving}
        >
          Save changes
        </button>
      </div>
    </div>
  );
}
