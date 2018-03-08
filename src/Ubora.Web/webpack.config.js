// webpack.config.test.js
const webpack = require('webpack');
const path = require('path');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

module.exports = env => {
    const isDevBuild = !(env && env.prod);

    return {
        // https://webpack.js.org/configuration/entry-context/
        entry: {
            app: './Scripts/app.js',
            drag_and_drop_file_uploads: './Scripts/modules/drag_and_drop_file_uploads.js',
            show_more: './Scripts/modules/show_more.js',
            toggle_formcheck_extra_fields: './Scripts/modules/toggle_formcheck_extra_fields.js',
            voting: './Scripts/modules/voting.js'
        },
        // https://webpack.js.org/configuration/output/
        output: {
            filename: '[name].bundle.js',
            path: path.resolve(__dirname, './wwwroot/dist')
        },
        resolve: {
            // https://webpack.js.org/configuration/resolve/#resolve-extensions
            extensions: ['.js', '.json', '.css', '.scss']
        },
        // https://webpack.js.org/configuration/externals/
        externals: {
            jquery: 'jquery',
            // bootstrap: 'bootstrap',
            'popper.js': {
                root: 'PopperJS',
                commonjs2: 'popper.js',
                commonjs: 'popper.js',
                amd: 'popper.js'
            }
        },
        // https://webpack.js.org/concepts/#loaders
        module: {
            // https://webpack.js.org/configuration/module/#rule
            rules: [
                {
                    test: /\.js$/,
                    use: [{
                        loader: 'babel-loader',
                        options: {
                            presets: [
                                ['env']
                            ]
                        }
                    }]
                },
                {
                    test: /\.css$/,
                    use: ExtractTextPlugin.extract({
                        use: [
                            {
                                loader: 'css-loader',
                                options: { importLoaders: 1, url: false } // "importLoader: 1" looks from postcss.config.js
                            },
                            { loader: 'postcss-loader' }
                        ]
                    })
                },
                {
                    test: /\.(scss)$/,
                    use: ExtractTextPlugin.extract({
                        //resolve-url-loader may be chained before sass-loader if necessary
                        use: [{
                            loader: "css-loader", // translates CSS into CommonJS
                            options: { importLoaders: 1, url: false }                
                        }, {
                            loader: "sass-loader" // compiles Sass to CSS
                        }]
                    })
                }
            ]
        },
        // https://webpack.js.org/concepts/#plugins & https://webpack.js.org/plugins/
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
                { from: './node_modules/select2/dist/css/select2.min.css', to: './lib' },
                { from: './node_modules/timeago/jquery.timeago.js', to: './lib' },
                { from: './node_modules/popper.js/dist/umd/popper.min.js', to: './lib/umd' },
                { from: './node_modules/popper.js/dist/umd/popper.min.js.map', to: './lib/umd' },
                { from: './node_modules/bootstrap/dist/js/bootstrap.min.js', to: './lib' },
                { from: './node_modules/bootstrap/dist/js/bootstrap.min.js.map', to: './lib' }
            ])
        ].concat(isDevBuild ? [
            // Develop plugins:
            new BundleAnalyzerPlugin({
                analyzerMode: 'static',
                reportFilename: 'report.html',
                openAnalyzer: false
            }),
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map'
            })
        ] : [
                // Production plugins:
                new UglifyJsPlugin({
                    parallel: true,
                    uglifyOptions: {
                        ecma: 8,
                        compress: true
                    },
                    extractComments: true
                })
            ])
    };
};