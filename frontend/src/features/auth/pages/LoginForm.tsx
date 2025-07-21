import { useState } from "react";
import "../styles/login.css";
import { Login } from "../../../api/authApi";

export const LoginForm = () => {
  const [identifier, setIdentifier] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(true);
  const [errors, setErrors] = useState<{
    identifier?: string;
    password?: string;
    general?: string;
  }>({});
  const [serverError, setServerError] = useState("");
  const [loading, setLoading] = useState(false);

  const validate = () => {
    const newErrors: typeof errors = {};
    if (!identifier.trim())
      newErrors.identifier = "Email or User name or phone is required";
    if (!password) newErrors.password = "Password is required";
    setErrors(newErrors);

    return Object.keys(newErrors).length === 0;
  };

  const handleSumbit = async (e: React.FormEvent) => {
    e.preventDefault();
    setServerError("");
    if (!validate()) return;

    setLoading(true);
    try {
      const result = await Login({ identifier, password, rememberMe });
      console.log("login Success : ", result);
      localStorage.setItem("accessToken", result.token);
    } catch (err: any) {
      const newError: typeof errors = {};
      newError.general = "Inavlid credentials";
      setErrors(newError);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <form className="login-form" onSubmit={handleSumbit}>
        <h2>Login</h2>

        <label>Email Or User name</label>
        <input
          placeholder="Type your email or username"
          value={identifier}
          onChange={(e) => setIdentifier(e.target.value)}
        />
        {errors.identifier && (
          <div className="error-text">{errors.identifier}</div>
        )}

        <label>Password</label>
        <input
          type="password"
          placeholder="Type your password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        {errors.password && <div className="error-text">{errors.password}</div>}

        <div className="remember-me">
          <input
            type="checkbox"
            checked={rememberMe}
            onChange={() => setRememberMe(!rememberMe)}
            id="rememberMe"
          />
          <label htmlFor="rememberMe">Remember Me</label>
        </div>

        {errors.general && <div className="error-text">{errors.general}</div>}

        <button type="submit" disabled={loading}>
          {loading ? "Logging in..." : "login"}
        </button>
      </form>
    </div>
  );
};
