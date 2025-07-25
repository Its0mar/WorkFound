import { useState } from "react";
import styles from '../styles/login.module.css'
import { Login } from "../../../api/authApi";
import { Link } from "react-router-dom";

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

  const handleSubmit = async (e: React.FormEvent) => {
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
    <div className={styles.loginContainer}>
      <form className={styles.loginForm} onSubmit={handleSubmit}>
        <h2>Login</h2>

        <label>Email Or User name</label>
        <input
          placeholder="Type your email or username"
          value={identifier}
          onChange={(e) => setIdentifier(e.target.value)}
        />
        {errors.identifier && (
          <div className={styles.errorText}>{errors.identifier}</div>
        )}

        <label>Password</label>
        <input
          type="password"
          placeholder="Type your password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        {errors.password && <div className={styles.errorText}>{errors.password}</div>}

        <div className={styles.rememberMe}>
          <input
            type="checkbox"
            checked={rememberMe}
            onChange={() => setRememberMe(!rememberMe)}
            id="rememberMe"
          />
          <label htmlFor="rememberMe">Remember Me</label>
        </div>

        {errors.general && <div className={styles.errorText}>{errors.general}</div>}

        <button type="submit" disabled={loading}>
          {loading ? "Logging in..." : "login"}
        </button>
        <hr className={styles.divider} />
        <div className={styles.loginLinks}>
          <Link to="/register">Don't have an account?</Link>
          <Link to="/reset-password-request">Forgot your password?</Link>
        </div>
      </form>
    </div>
  );
};
