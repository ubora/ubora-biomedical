Write-Host "Installing node module dependencies:"
npm install

Write-Host "Bundling with Webpack:"
node ./node_modules/webpack/bin/webpack.js --config webpack.config.js

Read-Host "Done! Press "Enter" to quit"
