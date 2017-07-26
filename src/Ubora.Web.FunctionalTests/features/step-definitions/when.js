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

    this.When(/^I log out$/, () => {
            browser
            .click('span=Menu')
            .click('button=Log out')
        });

    this.When(/^I sign up as "([^"]*)?" first name "([^"]*)?" last name "([^"]*)?"$/, (email, firstName, lastName) => {
            browser
            .click('a=Sign up')
            .setValue('#FirstName', firstName)
            .setValue('#LastName', lastName)
            .setValue('#Email', email)
            .setValue('#Password', 'Test12345')
            .setValue('#ConfirmPassword', 'Test12345')
            .click('#IsAgreedToTermsOfService')
            .click('button=Create an account')
        });

    this.When(/^I sign in as "([^"]*)?" with password "([^"]*)?"$/, (email, password) => {
            browser
            .click('a=Sign in/sign up')
            .setValue('#Email', email)
            .setValue('#Password', password)
            .click('button=Sign in')
        });
}