const expect = require('chai').expect;

module.exports = function () {
    this.Given(/^I clicked on the element "([^"]*)?"$/, (element) => {
            browser.click(element);
        });

    this.Given(/^I go to Home page$/, () => {
        browser.url('/');
    });
}