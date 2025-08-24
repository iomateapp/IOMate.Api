import { ref, computed, inject, provide } from 'vue';
import { useRouter } from 'vue-router';
import type { UserResponse } from '@/interfaces/users/UserResponse';
import type { TokenResponse } from '@/interfaces/users/TokenResponse';
import authorizationStorage from '@/services/authorizationStorage';

const AuthSymbol = Symbol('AuthContext');

export function provideAuth() {
    const user = ref<UserResponse | null>(null);
    const token = ref<string | null>(null);

    const isAuthenticated = computed(() => !!user.value && !!token.value);

    function setAuth(authPayload: { user: UserResponse; token: string }) {
        user.value = authPayload.user;
        token.value = authPayload.token;

        authorizationStorage.setTokens(authPayload.token);
    }

    function clearAuth() {
        user.value = null;
        token.value = null;
        authorizationStorage.clearTokens();
    }

    async function logout() {
        clearAuth();
        router.push({ path: '/login' });
    }

    // Interceptor para lidar com 401 
    function handleApiError(error: any) {
        if (error?.response?.status === 401) {
            logout();
        }
        throw error;
    }

    const router = useRouter();

    provide(AuthSymbol, {
        user,
        token,
        isAuthenticated,
        setAuth,
        clearAuth,
        logout,
        handleApiError,
    });
}

export function useAuth() {
    const context = inject<any>(AuthSymbol);
    if (!context) {
        throw new Error('Auth context not provided');
    }
    return context;
}
