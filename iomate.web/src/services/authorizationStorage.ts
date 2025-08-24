export default {
    setTokens(token: string) {
        localStorage.setItem('token', `Bearer ${token}`);
    },

    clearTokens() {
        localStorage.removeItem('token');
    },

    logout() {
        this.clearTokens();
    },

    getToken(): string | null {
        return localStorage.getItem('token');
    },
}