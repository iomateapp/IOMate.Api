export default {
    setTokens(accessToken: string, refreshToken: string) {
        localStorage.setItem('auth_token', `Bearer ${accessToken}`);
        localStorage.setItem('refresh_token', refreshToken);
    },

    clearTokens() {
        localStorage.removeItem('auth_token');
        localStorage.removeItem('refresh_token');
    },

    logout() {
        this.clearTokens();
    },

    getAccessToken(): string | null {
        return localStorage.getItem('auth_token');
    },

    getRefreshToken(): string | null {
        return localStorage.getItem('refresh_token');
    }
}