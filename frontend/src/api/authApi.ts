import axios from "axios";

export async function Login(data: {
  identifier: string;
  password: string;
  rememberMe: boolean;
}) {
  try {
    const response = await axios.post(
      "https://localhost:7246/api/Auth/Login",
      data,
      {
        headers: {
          "Content-Type": "application/json",
        },
        withCredentials: true,
      }
    );
    return response.data;
  } catch (error: any) {
    const msg =
      error.response?.data?.message || "Login failed. Please try again.";
    throw new Error(msg);
  }

}
