const webpack = require('webpack');
const path = require('path');

const merge = require('webpack-merge');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

module.exports = env => {
  const isDevBuild = !(env && env.prod);
  const config = {
    stats: { modules: false },
    context: __dirname,
    resolve: { extensions: ['.js', '.css'] },
    output: {
      filename: '[name].bundle.js',
      publicPath: '/'
    },
    module: {
      rules: [
        {
          test: /\.css$/,
          use: ExtractTextPlugin.extract({
            fallback: 'style-loader',
            use: {
              loader: 'css-loader', options: { importLoaders: 1 },
              loader: 'postcss-loader', options: { sourceMap: true }
            }
          })
        },
        { test: /\.js$/, use: { loader: 'babel-loader', options: { presets: ['env'] } } }
      ]
    },
    plugins: [
      new ExtractTextPlugin({
        filename: '[name].bundle.css',
        allChunks: true
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
    ]
  };

  const clientBundleOutputDir = './wwwroot/dist';
  const clientBundleConfig = merge(config, {
      entry: {
        'scripts': './Scripts/app.js',
        'styles': './Styles/styles.css'
      },
      output: { path: path.join(__dirname, clientBundleOutputDir) },
      plugins: [].concat(isDevBuild ? [
        new BundleAnalyzerPlugin({
          analyzerMode: 'static',
          reportFilename: 'report.html',
          openAnalyzer: false
        }),
        new webpack.SourceMapDevToolPlugin({
          filename: '[file].map',
          moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]')
        })
      ] : [
        new UglifyJsPlugin({
          parallel: true,
          uglifyOptions: {
            ecma: 8,
            compress: true
          },
          extractComments: true
        })
      ])
    });

    return clientBundleConfig;
  };
