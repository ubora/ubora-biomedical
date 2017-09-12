const webpack = require('webpack');
const path = require('path');

const UglifyJSPlugin = require('uglifyjs-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
  entry: './wwwroot/build/js/app.js',
  output: {
    filename: 'scripts.bundle.js',
    path: path.join(__dirname, 'wwwroot/dist'),
    publicPath: '/'
  },
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
      filename: './styles.bundle.css'
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
      minChunks(module, count) {
        let context = module.context;
        return context && context.indexOf('node_modules') >= 0;
      }
    })
  ]
}
