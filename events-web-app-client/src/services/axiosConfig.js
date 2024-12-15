import { jwtDecode } from "jwt-decode";
import axios from "axios";
import { refreshAccessToken } from "./authService";

const api = axios.create({
    baseURL: "https://localhost:7178/api",
});

const isTokenExpired = (token) => {
    try {
        const { exp } = jwtDecode(token);
        return Date.now() >= exp * 1000;
    } catch (err) {
        console.error("Ошибка декодирования токена:", err);
        return true;
    }
};

api.interceptors.request.use(
    async (config) => {
        let token = localStorage.getItem("accessToken");

        if (token && isTokenExpired(token)) {
            token = await refreshAccessToken();
        }

        if (token) {
            config.headers["Authorization"] = `Bearer ${token}`;
        }

        return config;
    },
    (error) => Promise.reject(error)
);

api.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;
        if (error.response?.status === 401 && !originalRequest._retry && !originalRequest.url.includes('/User/login')) {
            originalRequest._retry = true;

            try {
                const newAccessToken = await refreshAccessToken();

                if (newAccessToken) {
                    originalRequest.headers["Authorization"] = `Bearer ${newAccessToken}`;
                    return api(originalRequest);
                }
            } catch (err) {
                console.error("Ошибка обновления токена:", err);
            }
        }

        return Promise.reject(error);
    }
);

export default api;
