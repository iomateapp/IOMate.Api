import { apiFetch } from "./client"

export type LoginResponse = {
    token: string
    refreshToken: string
}

export const loginRequest = async (email: string, password: string): Promise<LoginResponse> => {
    return apiFetch<LoginResponse>('/auth', {
        method: 'POST',
        body: JSON.stringify({ email, password }),
    })
}

