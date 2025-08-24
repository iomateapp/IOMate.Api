import api from '@/api/api';

export interface AuthRequestDto {
  email: string;
  password: string;
}

export interface AuthResponseDto {
  token: string;
}

const API_URL = '/api/auth';

const authService = {
  authenticate(payload: AuthRequestDto): Promise<AuthResponseDto> {
    return api.post(`${API_URL}`, payload).then(res => res.data);
  }
};

export default authService;
