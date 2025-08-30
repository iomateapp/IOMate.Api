export type PostType = {
    Message: string
    ValidationErrors: ValidationError[]
}

export type ValidationError = {
    Field: string
    Message: string
}

export type LoginResponse = {
    token: string
}

const lang = navigator.language || 'en-US'
const baseUrl = 'http://localhost:5208/api'

export const loginRequest = async (email: string, password: string): Promise<LoginResponse> => {
  const response = await fetch(`${baseUrl}/auth`, {
    method: 'POST',
    body: JSON.stringify({ email, password }),
    headers: { 
        'Content-Type': 'application/json',
        'Accept-Language': lang
     },
  })
  return await response.json()
}
