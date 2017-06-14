var cucumber = require('cucumber');
var fs = require('fs');
var uuid = require('uuid');

var hooks = function () {


    this.After(function (scenario, callback) {
        if (scenario.isFailed()) {
            var buffer = browser.saveScreenshot();
            scenario.attach(new Buffer(buffer, 'base64'), 'image/png')
            callback();
        } else {
            callback();
        }
    });

    var jsonReports = process.cwd() + '/reports/jsons';
    var JsonFormatter = cucumber.Listener.JsonFormatter();
    JsonFormatter.log = function (string) {
        if (!fs.existsSync(jsonReports)) {
            fs.mkdirSync(jsonReports);
        }

        var targetJson = jsonReports + '/cucumber_report' + uuid.v1() + '.json';

        return fs.writeFileSync(targetJson, string);
    };
    this.registerListener(JsonFormatter);
}
module.exports = hooks;