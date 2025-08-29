import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import { tanstackRouter } from '@tanstack/router-plugin/vite'

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [
        tanstackRouter({
            target: 'react',
            autoCodeSplitting: true,
        }),
        plugin(),
    ],
    server: {
        port: 56717,
    }
})