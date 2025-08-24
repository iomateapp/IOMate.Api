import api from '@/api/api';

export interface UserDto {
  id?: string;
  email: string;
  firstName: string;
  lastName: string;
  // outros campos conforme necessário
}

export interface CreateUserRequestDto {
  email: string;
  firstName: string;
  lastName: string;
  password: string;
}

export interface UpdateUserRequestDto {
  email?: string;
  firstName?: string;
  lastName?: string;
  // outros campos editáveis
}

const API_URL = '/api/users';

const userService = {
  getAll(pageNumber = 1, pageSize = 10) {
    return api.get(`${API_URL}?pageNumber=${pageNumber}&pageSize=${pageSize}`).then(res => res.data);
  },

  create(payload: CreateUserRequestDto) {
    return api.post(`${API_URL}`, payload).then(res => res.data);
  },

  update(id: string, payload: UpdateUserRequestDto) {
    return api.put(`${API_URL}/${id}`, payload).then(res => res.data);
  },

  delete(id: string) {
    return api.delete(`${API_URL}/${id}`).then(res => res.data);
  }
};

export default userService;
