var cucumber = require('cucumber');
var fs = require('fs');
var reporter = require('cucumber-html-reporter');
var uuid = require('uuid');

var hooks = function () {

    var jsonReports = process.cwd() + '/reports/jsons';
    var htmlReports = process.cwd() + '/reports';

    this.After(function (scenario, callback) {
        if (scenario.isFailed()) {
            var buffer = browser.saveScreenshot();
            scenario.attach(new Buffer(buffer, 'base64'), 'image/png')
            callback();
        } else {
            callback();
        }
    });

    var options = {
        brandTitle: "Smoke Tests Report",
        name: 'Ubora project',
        theme: 'bootstrap',
        jsonDir: jsonReports,
        output: htmlReports + '/cucumber_html_reporter.html',
        reportSuiteAsScenarios: true
    };

    var JsonFormatter = cucumber.Listener.JsonFormatter();
    JsonFormatter.log = function (string) {
        if (!fs.existsSync(jsonReports)) {
            fs.mkdirSync(jsonReports);
        }

        var targetJson = jsonReports + '/cucumber_report' + uuid.v1() + '.json';
        fs.writeFileSync(targetJson, string);
        return reporter.generate(options);
    };
    this.registerListener(JsonFormatter);
}
module.exports = hooks;