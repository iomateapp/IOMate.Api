import type { AuthPayload } from '@/interfaces/users/AuthPayload';
import type { TokenResponse } from '@/interfaces/users/TokenResponse';
import type { ConfirmEmailParams } from '@/interfaces/users/ConfirmEmailParams';
import type { Manage2faPayload } from '@/interfaces/users/Manage2faPayload';
import api from '@/api/api';
import type { UserResponse } from '@/interfaces/users/UserResponse';

const API_URL = '/api/users';

export default {
    register(payload: AuthPayload): Promise<any> {
        return api.post(`${API_URL}/register`, payload).then(res => res.data);
    },

    login(payload: AuthPayload): Promise<TokenResponse> {
        return api.post(`${API_URL}/login`, payload).then(res => res.data);
    },

    refresh(payload: { refreshToken: string }): Promise<TokenResponse> {
        return api.post(`${API_URL}/refresh`, payload).then(res => res.data);
    },

    confirmEmail(params: ConfirmEmailParams): Promise<any> {
        return api.get(`${API_URL}/confirmEmail`, { params }).then(res => res.data);
    },

    resendConfirmationEmail(payload: { email: string }): Promise<any> {
        return api.post(`${API_URL}/resendConfirmationEmail`, payload).then(res => res.data);
    },

    forgotPassword(payload: { email: string }): Promise<any> {
        return api.post(`${API_URL}/forgotPassword`, payload).then(res => res.data);
    },

    resetPassword(payload: { email: string; token: string; newPassword: string }): Promise<any> {
        return api.post(`${API_URL}/resetPassword`, payload).then(res => res.data);
    },

    manage2fa(payload: Manage2faPayload): Promise<any> {
        return api.post(`${API_URL}/manage/2fa`, payload).then(res => res.data);
    },

    getInfo(): Promise<any> {
        return api.get(`${API_URL}/manage/info`).then(res => res.data);
    },

    updateInfo(payload: any): Promise<any> {
        return api.post(`${API_URL}/manage/info`, payload).then(res => res.data);
    },

    getUsers(): Promise<UserResponse[]> {
        return api.get(`${API_URL}`).then(res => res.data);
    },

    updateUser(payload: any, id: string): Promise<UserResponse> {
        return api.put(`${API_URL}/${id}`, payload).then(res => res.data);
    },
};
