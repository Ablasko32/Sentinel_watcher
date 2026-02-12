import { definePreset } from '@primeuix/themes';
import Aura from '@primeuix/themes/Aura';

export const SentinelPreset = definePreset(Aura, {
  semantic: {
    primary: {
      50: '#f5f5f4',
      100: '#efefee',
      200: '#e6e6e6',
      300: '#d1d1d1',
      400: '#9a9a9a',
      500: '#0b0b0b', // light-mode primary (almost black)
      600: '#0a0a0a',
      700: '#080808',
      800: '#060606',
      900: '#040404',
      950: '#020202',
    },
    colorScheme: {
      light: {
        primary: {
          color: '#0b0b0b',
          inverseColor: '#ffffff',
          hoverColor: '#222222',
          activeColor: '#111111',
        },
        highlight: {
          background: '#f4f4f3',
          focusBackground: '#e9e9e8',
          color: '#0b0b0b',
          focusColor: '#222222',
        },
      },
      dark: {
        primary: {
          color: '#00ff66',
          inverseColor: '#021008',
          hoverColor: '#00e659',
          activeColor: '#00cc4d',
        },
        highlight: {
          background: 'rgba(0, 255, 102, 0.12)',
          focusBackground: 'rgba(0, 255, 102, 0.18)',
          color: 'rgba(255,255,255,.95)',
          focusColor: 'rgba(255,255,255,.95)',
        },
      },
    },
  },
});
