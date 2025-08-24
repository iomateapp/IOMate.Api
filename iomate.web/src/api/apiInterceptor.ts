
import api from '@/api/api'; // seu axios instance
import authorizationStorage from '../services/authorizationStorage';

api.interceptors.request.use(
    config => {
        const token = authorizationStorage.getToken();
        if (token) {
            config.headers['Authorization'] = token;
        }
        return config;
    },
    error => Promise.reject(error)
);

api.interceptors.response.use(
    response => response,
    error => {
        if (error.response?.status === 401) {
            authorizationStorage.logout();
        }
        return Promise.reject(error);
    }
);
