const expect = require('chai').expect;

module.exports = function () {
    this.When(
        /^I click on the element "([^"]*)?"$/, (element) => {
            browser.click(element);
        });

    this.When(
        /^I set "([^"]*)?" to the element "([^"]*)?"$/, (value, element) => {
            browser.setValue(element, value);
        });

    this.When(
        /^I add "([^"]*)?" to the element "([^"]*)?"$/, (value, element) => {
            browser.addValue(element, value);
        });
}