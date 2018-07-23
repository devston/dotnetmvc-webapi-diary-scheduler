/// <binding ProjectOpened='Watch - Development' />
var webpack = require("webpack");
var WebpackNotifierPlugin = require("webpack-notifier");
var MiniCssExtractPlugin = require("mini-css-extract-plugin");
var path = require("path");

module.exports = {
    // configuration
    mode: "development",
    // Turn off caching for now as there is a bug in the mini css extract plugin that does not
    // recompile css changes unless webpack is restarted.
    // Remove this when this bug has been fixed.
    cache: false,
    entry: {
        main: "./Scripts/Pages/main.ts"
    },
    output: {
        path: path.join(__dirname, "./dist"),
        filename: "[name].bundle.js"
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                loader: "babel-loader",
                options: {
                    presets: [require("babel-preset-env")],
                    cacheDirectory: "" // tells the loader to use default cache directory
                },
                include: [
                    path.resolve(__dirname, "./Scripts/")
                ]
            },
            {
                test: /\.tsx?$/,
                loader: "ts-loader",
                options: {
                    transpileOnly: true,
                },
                include: [
                    path.resolve(__dirname, "./Scripts/")
                ]
            },
            {
                test: /\.json$/,
                loader: "json-loader"
            },
            {
                // Regular css files.
                test: /\.css$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    { loader: "css-loader", options: { minimize: true } }
                ]
            },
            {
                // sass / scss files.
                test: /\.(sass|scss)$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: "css-loader",
                        options: {
                            minimize: true
                        }
                    },
                    {
                        loader: "sass-loader",
                        options: {
                            includePaths: [
                                path.resolve(__dirname, "../Content/Static/")
                            ]
                        }
                    }
                ]
            },
            {
                // The url-loader uses DataUrls. 
                test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: "url-loader?limit=10000&mimetype=application/font-woff"
            },
            {
                // The file-loader emits files.
                test: /\.(ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: "file-loader"
            },
            {
                test: /\.handlebars$/,
                loader: "handlebars-loader?helperDirs[]=" + __dirname + "/Scripts/HandlebarsHelpers"
            },
            {
                // Force jquery-validation-unobtrusive to use CommonJS instead of AMD.
                test: /[\/\\]node_modules[\/\\]jquery-validation-unobtrusive[\/\\]dist[\/\\]jquery.validate.unobtrusive\.js$/,
                loader: "imports-loader?define=>false"
            }
            // This adds eslint support . Commented for now to improve webpack build speed.
            //{
            //    enforce: 'pre',
            //    test: /\.js$/,
            //    exclude: [/node_modules/],
            //    loader: 'eslint-loader',
            //    options: {
            //        fix: true             
            //    }
            //}
        ]
    },
    plugins: [
        new WebpackNotifierPlugin({
            alwaysNotify: true,
            messageFormatter: function (a, b) {
                return "Error in webpack build";
            }
        }), // send notification in visual studio of successful build
        new webpack.ProvidePlugin({
            $: "jquery",
            "jQuery": "jquery",
            "window.jQuery": "jquery",
            "window.$": "jquery"
        }),
        new webpack.SourceMapDevToolPlugin({
            filename: "[file].map",
            exclude: ["vendor.bundle.js"]
        }),
        new MiniCssExtractPlugin({
            // Define where to save the css.
            filename: "site.bundle.min.css"
        })
    ],
    resolve: {
        extensions: [".tsx", ".ts", ".webpack.js", ".web.js", ".js", ".handlebars"],
        alias: {
            Scripts: path.resolve(__dirname, "./Scripts/"),
            Content: path.resolve(__dirname, "./Content/"),
            handlebars: "handlebars/dist/handlebars.min.js",
            handlebarsTemplates: path.resolve(__dirname, "Scripts/Templates")
        }
    },
    target: "web",
    devServer: {
        stats: {
            errors: false,
            errorDetails: false,
            warnings: false,
            publicPath: false
        }
    }
};