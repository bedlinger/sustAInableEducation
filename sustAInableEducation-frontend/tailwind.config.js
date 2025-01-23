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
        'anim': 'anim 25s linear infinite',
      },
      keyframes: {
        anim: {
          '0%': { left: '0px', top: '0px' },
          '12%': { rotate: '45deg' },
          '25%': { left: '700px', top: '150px', rotate: '90deg' },
          '38%': { rotate: '135deg' },
          '50%': { left: '680px', top: '280px', rotate: '180deg' },
          '62%': { rotate: '225deg' },
          '75%': { left: '300px', top: '300px', rotate: '270deg' },
          '88%': { rotate: '315deg' },
          '100%': { left: '0px', top: '0px', rotate: '360deg' },
        },
      },
    },
  },
  plugins: [require('@tailwindcss/typography')],
}

