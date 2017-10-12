module.exports = {
  parser: 'postcss-safe-parser',
  plugins: {
    'postcss-import': {},
    'postcss-cssnext': {
      features: {
        attributeCaseInsensitive: false
      }
    },
    'cssnano': {}
  }
};
