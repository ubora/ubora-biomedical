Write-Host "Installing node module dependencies:"
npm install

Write-Host "Bundling and watching with Webpack:"
node ./node_modules/webpack/bin/webpack.js --watch --config webpack.config.js
