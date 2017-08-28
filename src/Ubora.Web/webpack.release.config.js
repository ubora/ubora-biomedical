const webpack = require('webpack');
const path = require('path');

const UglifyJSPlugin = require('uglifyjs-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
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
    filename: 'scripts.min.js'
  },
  module: {
    loaders: [
      {
				test: /\.css$/,
				use: ExtractTextPlugin.extract({
					fallback: "style-loader",
					use: [
            {
              loader: 'css-loader',
              options: { importLoaders: 1 }
            },
            'postcss-loader'
          ]
				})
			},
      {
        test: /\.js$/,
        exclude: /(node_modules|bower_components)/,
        use: {
          loader: 'babel-loader',
          options: {
            presets: ['env']
          }
        }
      }
    ]
  },
  plugins: [
    new ExtractTextPlugin({
      filename: './styles.min.css'
    }),
    new UglifyJSPlugin({
      parallel: true,
      uglifyOptions: {
        ecma: 8,
        compress: true
      },
      extractComments: true
    }),
    new webpack.optimize.CommonsChunkPlugin({
      name: 'vendors',
      filename: 'vendors.js',
      minChunks: Infinity
    })
  ]
}
