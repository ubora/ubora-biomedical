const expect = require('chai').expect;

module.exports = function () {
    this.When(/^I click on the element "([^"]*)?"$/, (element) => {
            browser.click(element);
        });

    this.When(/^I set value "([^"]*)?" to the element "([^"]*)?"$/, (value, element) => {
            browser.setValue(element, value);
        });

    this.When(/^I select value "([^"]*)?" from element "([^"]*)?"$/, (value, element) => {
            browser.selectByValue(element,value);
        });

    this.When(/^I click on the key "([^"]*)?"$/, (value) => {
            browser.keys(value);
        });

    this.When(/^I go back to last page$/, () => {
            browser.back();
        });
    
    this.When(/^I click on keys "([^"]*)?"$/, (value) => {
            browser.keys(value);
        });
}