Write-Host "Installing node module dependencies:"
npm install

Write-Host "Running front-end vendor tasks:"
node ./node_modules/webpack/bin/webpack.js --config webpack.config.vendor.js

Write-Host "Running front-end bundle tasks:"
node ./node_modules/webpack/bin/webpack.js --config webpack.config.js

Read-Host "Done! Press "Enter" to quit"
