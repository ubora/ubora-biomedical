const webpack = require('webpack');
const path = require('path');

const ExtractTextPlugin = require('extract-text-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

module.exports = {
  entry: './wwwroot/build/app.js',
  output: {
    filename: '[name].dev.bundle.js',
    path: path.join(__dirname, 'wwwroot/dist'),
    publicPath: '/'
  },
  devtool: 'inline-source-map',
  resolve: {
    alias: {
      'jquery': './node_modules/jquery/dist/jquery.min.js',
      'jquery-validation': './node_modules/jquery-validation/dist/jquery.validate.min.js',
      'jquery-validation-unobtrusive': './node_modules/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js',
      'autocomplete': './node_modules/autocomplete-js/dist/autocomplete.min.js',
      'simplemde-scripts': './node_modules/simplemde/dist/simplemde.min.js',
      'simplemde-styles': './node_modules/simplemde/dist/simplemde.min.css',
      'marked': './node_modules/marked/marked.min.js'
    },
    modules: [
      path.resolve('./'),
      path.resolve('./node_modules')
    ]
  },
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
    new webpack.optimize.CommonsChunkPlugin({
      name: 'vendors',
      filename: 'vendors.js',
      minChunks(module, count) {
        let context = module.context;
        return context && context.indexOf('node_modules') >= 0;
      }
    }),
    new BundleAnalyzerPlugin({
      analyzerMode: 'static',
      reportFilename: 'report.html',
      openAnalyzer: false
    })
  ]
}
