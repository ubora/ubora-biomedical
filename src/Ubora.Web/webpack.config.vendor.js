const webpack = require('webpack');
const path = require('path');

const ExtractTextPlugin = require('extract-text-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const merge = require('webpack-merge');

const vendors = [
  'jquery',
  'jquery-validation',
  'jquery-validation-unobtrusive',
  'autocomplete-js',
  'simplemde',
  'marked',
  'select2'
];

module.exports = env => {
  const extractCSS = new ExtractTextPlugin('vendor.css');
  const isDevBuild = !(env && env.prod);
  const sharedConfig = {
    stats: { modules: false },
    resolve: { extensions: ['.js'] },
    module: {
      rules: [ ]
    },
    output: {
      publicPath: 'dist/',
      filename: '[name].js',
      library: '[name]_[hash]'
    },
    plugins: [
      new webpack.ProvidePlugin({
        $: 'jquery',
        jQuery: 'jquery'
      })
    ]
  };

  const clientBundleConfig = merge(sharedConfig, {
    entry: {
      vendor: vendors
    },
    output: { path: path.join(__dirname, 'wwwroot', 'dist') },
    module: {
      rules: [
        { test: /\.css(\?|$)/, use: extractCSS.extract({ use: isDevBuild ? 'css-loader' : 'css-loader?minimize' }) },
        { test: require.resolve('jquery'), loader: 'expose-loader?$'}
      ]
    },
    plugins: [
      extractCSS,
      new webpack.DllPlugin({
        path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
        name: '[name]_[hash]'
      }),
      new CopyWebpackPlugin([
        { from: './node_modules/jquery/dist/jquery.min.js', to: './lib' },
        { from: './node_modules/jquery-validation/dist/jquery.validate.min.js', to: './lib' },
        { from: './node_modules/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js', to: './lib' },
        { from: './node_modules/autocomplete-js/dist/autocomplete.min.js', to: './lib' },
        { from: './node_modules/autocomplete-js/dist/autocomplete.min.css', to: './lib' },
        { from: './node_modules/marked/marked.min.js', to: './lib' },
        { from: './node_modules/simplemde/dist/simplemde.min.js', to: './lib' },
        { from: './node_modules/simplemde/dist/simplemde.min.css', to: './lib' },
        { from: './node_modules/select2/dist/js/select2.min.js', to: './lib' },
        { from: './node_modules/select2/dist/css/select2.min.css', to: './lib' }
      ])
    ].concat(isDevBuild ? [] : [
      new webpack.optimize.UglifyJsPlugin()
    ])
  });

  return clientBundleConfig;

};
