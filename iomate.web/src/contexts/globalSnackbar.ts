import { ref, provide, inject } from 'vue';

const SnackbarSymbol = Symbol('GlobalSnackbar');

export function provideGlobalSnackbar() {
    const show = ref(false);
    const message = ref('');
    const color = ref('');
    const timeout = ref(5000);
    const title = ref('');

    function notify(msg: string, options: { color?: string; timeout?: number; title?: string } = {}) {
        message.value = msg;
        color.value = options.color || 'info';
        timeout.value = options.timeout || 5000;
        title.value = options.title || '';
        show.value = true;
    }

    const snackbar = { show, message, color, timeout, title, notify };
    provide(SnackbarSymbol, snackbar);
    return snackbar;
}

export function useGlobalSnackbar() {
    const ctx = inject<any>(SnackbarSymbol);
    if (!ctx) throw new Error('GlobalSnackbar not provided');
    return ctx;
}
