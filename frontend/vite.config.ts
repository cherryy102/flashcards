import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import eslint from 'vite-plugin-eslint';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(), eslint()],
  build: {
    target: 'esnext',
    sourcemap: true,
  },
  resolve: {
    alias: [{ find: '@', replacement: '/src' }],
  },
  server: {
    port: 8118,
    proxy: {
      '/api/flashcards': {
        target: 'https://localhost:9092',
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace('/api/flashcards', '/api/flashcards'),
      },
      '/api': {
        target: 'https://localhost:9092',
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace('/api', ''),
      },
    },
  },
});
