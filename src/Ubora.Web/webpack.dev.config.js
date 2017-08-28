const webpack = require('webpack');
const path = require('path');

const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
  devtool: 'source-map',
  watch: false,
  entry: {
    scripts: [
      './wwwroot/build/app.js'
    ],
    vendors: [
      './node_modules/jquery/dist/jquery.min.js',
      './node_modules/jquery-validation/dist/jquery.validate.min.js',
      './node_modules/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js',
      './node_modules/autocomplete-js/dist/autocomplete.min.js',
      './node_modules/simplemde/dist/simplemde.min.js',
      './node_modules/simplemde/dist/simplemde.min.css',
      './node_modules/marked/marked.min.js'
    ]
  },
  output: {
    publicPath: '/',
    path: path.join(__dirname, 'wwwroot/dist'),
    filename: 'scripts.dev.js'
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
      minChunks: Infinity
    })
  ]
}
