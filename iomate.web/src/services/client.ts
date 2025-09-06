import { ApiError, type ApiErrorResponse } from "./errors/types"

const lang = navigator.language || 'en-US'
const baseUrl = 'http://localhost:5208/api'

export const apiFetch = async <T>(url: string, options: RequestInit = {}, token: string = ""): Promise<T> => {
    const response = await fetch(`${baseUrl}${url}`, {
        ...options,
        headers: {
        ...(options.headers || {}),
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
        'Accept-Language': lang,
        'Content-Type': 'application/json'
        },
    })

    if (!response.ok) {
        let errorBody: ApiErrorResponse | undefined;

        try {
            errorBody = await response.json();
        } catch {
            // Undefined if there's no body (should never happen)
        }

        throw new ApiError("Request failed", errorBody ?? { 
            Message: response.statusText, 
            ValidationErrors: [] 
        });
    }

    return await response.json();
}
