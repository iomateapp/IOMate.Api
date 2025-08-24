import api from '@/api/api'; // seu axios instance
import userService from '@/services/userService';
import authorizationStorage from '../services/authorizationStorage';

let isRefreshing = false;
let failedQueue: any[] = [];

const processQueue = (error: any, token: string | null = null) => {
    failedQueue.forEach(prom => {
        if (error) {
            prom.reject(error);
        } else {
            prom.resolve(token);
        }
    });

    failedQueue = [];
};

api.interceptors.request.use(
    config => {
        const token = authorizationStorage.getAccessToken();
        if (token) {
            config.headers['Authorization'] = token;
        }
        return config;
    },
    error => {
        return Promise.reject(error);
    }
);

api.interceptors.response.use(
    response => response,
    async error => {
        const originalRequest = error.config;

        if (error.response?.status === 401 && !originalRequest._retry) {
            if (isRefreshing) {
                return new Promise(function (resolve, reject) {
                    failedQueue.push({ resolve, reject });
                })
                    .then(token => {
                        originalRequest.headers['Authorization'] = token;
                        return api(originalRequest);
                    })
                    .catch(err => Promise.reject(err));
            }

            originalRequest._retry = true;
            isRefreshing = true;

            const refreshToken = authorizationStorage.getRefreshToken();

            if (!refreshToken) {
                authorizationStorage.logout();
                return Promise.reject(error);
            }

            try {
                const data = await userService.refresh({ refreshToken });
                const newToken = `Bearer ${data.accessToken} `;
                authorizationStorage.setTokens(data.accessToken, data.refreshToken);

                api.defaults.headers.common['Authorization'] = newToken;
                originalRequest.headers['Authorization'] = newToken;
                processQueue(null, newToken);
                return api(originalRequest);
            } catch (err) {
                processQueue(err, null);
                authorizationStorage.logout();
                return Promise.reject(err);
            } finally {
                isRefreshing = false;
            }
        }

        return Promise.reject(error);
    }
);
