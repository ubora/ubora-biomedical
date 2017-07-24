const path = require("path");
const webpack = require("webpack");

module.exports = {
    entry: [
        "./wwwroot/js/UBORA.js"
    ],
    output: {
        publicPath: "/",
        path: path.join(__dirname, "wwwroot/dist"),
        filename: "UBORA.min.js"
    },
    watch: true,
    module: {
        loaders: [
            {
                test: /\.js$/,
                use: {
                    loader: "babel-loader",
                    options: {
                        presets: ["es2017"]
                    }
                },
                exclude: /(node_modules|bower_components)/,
                include: __dirname
            }
        ]
    }
}