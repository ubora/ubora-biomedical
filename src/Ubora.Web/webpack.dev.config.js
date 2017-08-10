
const path = require('path');
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    devtool: 'source-map',
    stats: {
        colors: true
    },
    watch: true,
    watchOptions: {
        poll: true
    },
    entry: {
        app: [ './wwwroot/build/app.js' ]
    },
    output: {
        publicPath: '/',
        path: path.join(__dirname, 'wwwroot/dist'),
        filename: '[name].min.js'
    },
    plugins: [
        new UglifyJSPlugin({
            parallel: true,
            uglifyOptions: {
                ecma: 7,
                compress: true
            },
            extractComments: true
        }),
        new ExtractTextPlugin('app.css')
    ],
    module: {
      rules: [
        {
            test: /\.js$/,
            exclude: /node_modules|bower_components/,
            loader: 'babel-loader',
            query: {
                presets: ['env', 'es2017']
            }
        },
        {
        test: /\.css$/,
        use: [
          'style-loader',
          { loader: 'css-loader', options: { importLoaders: 1 } },
          'postcss-loader'
        ]
      }
      ]
    }
};
