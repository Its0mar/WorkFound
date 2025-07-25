import { Routes, Route } from "react-router-dom";
import { LoginForm } from "./features/auth/pages/LoginForm";
import PublicRoute from "./components/PublicRoute";
import { ResetPasswordRequest } from "./features/auth/pages/ResetPasswordRequest";
import { ToastContainer } from "react-toastify";
import { ResetPasswordForm } from "./features/auth/pages/ResetPasswordForm";
import { RegisterForm } from "./features/auth/pages/RegisterForm";

function App() {
  return (
    <>
      <Routes>
        <Route element={<PublicRoute />}>
          <Route path="/login" element={<LoginForm />} />
          <Route
            path="/reset-password-request"
            element={<ResetPasswordRequest />}
          />
          <Route path="/reset-password" element={<ResetPasswordForm/>}/>
          <Route path="/register" element={<RegisterForm/>}/>
        </Route>
      </Routes>
      <ToastContainer position="top-right" autoClose={3000} />
    </>
  );
}

export default App;
