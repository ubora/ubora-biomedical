const webpack = require('webpack');
const path = require('path');

const ExtractTextPlugin = require('extract-text-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

module.exports = {
  entry: {
    scripts: './wwwroot/build/js/app.js',
    vendors: [
      'simplemde',
      'marked',
      'autocomplete-js',
      'jquery-validation-unobtrusive',
      'jquery-validation',
      'jquery'
    ]
  },
  output: {
    filename: '[name].dev.js',
    path: path.join(__dirname, 'wwwroot/dist'),
    publicPath: '/'
  },
  devtool: 'inline-source-map',
  module: {
    loaders: [
      {
				test: /\.css$/,
				use: ExtractTextPlugin.extract({
					fallback: "style-loader",
					use: 'css-loader'
				})
			}
    ],
  },
  plugins: [
    new ExtractTextPlugin({ filename: './styles.dev.css' }),
    new BundleAnalyzerPlugin({
      analyzerMode: 'static',
      reportFilename: 'report.html',
      openAnalyzer: false
    }),
    new webpack.optimize.CommonsChunkPlugin({
      name: 'vendors',
      minChunks: 3
    }),
    new webpack.ProvidePlugin({
      $: 'jquery',
      jQuery: 'jquery',
      'jquery.validate': 'jquery-validation',
      'jquery.unobtrusive': 'jquery-validation-unobtrusive',
      autocomplete: 'autocomplete-js',
      marked: 'marked',
      SimpleMDE: 'simplemde'
    })
  ]
}
