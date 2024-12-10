import { updatePrimaryPalette } from '@primevue/themes';

/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./components/**/*.{js,vue,ts}",
    "./layouts/**/*.vue",
    "./pages/**/*.vue",
    "./plugins/**/*.{js,ts}",
    "./app.vue",
    "./error.vue",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#F3FAF3',
          100: '#E3F5E3',
          200: '#C8EAC9',
          300: '#9DD89F',
          400: '#6ABE6E',
          500: '#45A249',
          600: '#388E3C',
          700: '#2C692F',
          800: '#275429',
          900: '#224525',
          950: '#0E2511'
        }
      },
      animation: {
        'spin-slow': 'spin 3s linear infinite',
      },
      keyframes: {
        spin: {
          '0%, 100%': { transform: 'rotate(-3deg)' },
          '50%': { transform: 'rotate(3deg)' },
        },
      },
    },
  },
  plugins: [],
}

