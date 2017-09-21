const webpack = require('webpack');
const path = require('path');

const ExtractTextPlugin = require('extract-text-webpack-plugin');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin')
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const merge = require('webpack-merge');

module.exports = env => {
  const isDevBuild = !(env && env.prod);
  const sharedConfig = {
    stats: { modules: false },
    context: __dirname,
    resolve: { extensions: ['.js'] },
    output: {
      filename: '[name].bundle.js',
      publicPath: '/'
    },
    module: {
      rules: [
        { test: /\.css$/, use: [{ loader: 'style-loader' }, { loader: 'css-loader', options: { importLoaders: 1 } }, { loader: 'postcss-loader' } ] },
        { test: /\.js$/, use: { loader: 'babel-loader', options: { presets: ['env'] } } }
      ]
    },
    plugins: [ ]
  };

  const clientBundleOutputDir = './wwwroot/dist';
  const clientBundleConfig = merge(sharedConfig, {
    entry: { 'scripts': './Scripts/app.js' },
    output: { path: path.join(__dirname, clientBundleOutputDir) },
    plugins: [
      new webpack.DllReferencePlugin({
        context: __dirname,
        manifest: require('./wwwroot/dist/vendor-manifest.json')
      })
    ].concat(isDevBuild ? [
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
      new ExtractTextPlugin({ filename: './[name].bundle.css' }),
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
