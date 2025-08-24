
import { ref, computed, inject, provide } from 'vue';
import { useRouter } from 'vue-router';
import authorizationStorage from '@/services/authorizationStorage';

const AuthSymbol = Symbol('AuthContext');

export function provideAuth() {
    const token = ref<string | null>(authorizationStorage.getToken());
    const isAuthenticated = computed(() => !!token.value);

    function setAuth(authPayload: { token: string }) {
        token.value = authPayload.token;
        authorizationStorage.setTokens(authPayload.token);
    }

    function clearAuth() {
        token.value = null;
        authorizationStorage.clearTokens();
    }

    const router = useRouter();

    async function logout() {
        clearAuth();
        router.push({ path: '/login' });
    }

    provide(AuthSymbol, {
        token,
        isAuthenticated,
        setAuth,
        clearAuth,
        logout,
    });
}

export function useAuth() {
    const context = inject<any>(AuthSymbol);
    if (!context) {
        throw new Error('Auth context not provided');
    }
    return context;
}
