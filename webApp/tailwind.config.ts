import type { Config } from 'tailwindcss'

const config: Config = {
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        surface: '#f6f7fb',
        ink: '#0f172a',
      },
    },
  },
  plugins: [],
}

export default config
