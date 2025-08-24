import type { App } from 'vue';
import { provideAuth } from '@/contexts/authContext';

export function registerAuthContext(app: App) {
    // Chama o provideAuth no setup global
    app.mixin({
        setup() {
            provideAuth();
        },
    });
}
