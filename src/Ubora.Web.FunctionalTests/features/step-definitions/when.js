const expect = require('chai').expect;

module.exports = function () {
    this.When(/^I click on the element "([^"]*)?"$/, (element) => {
            browser.waitForEnabled(element, 1500);
            browser.click(element);
        });

    this.When(/^I set value "([^"]*)?" to the element "([^"]*)?"$/, (value, element) => {
            browser.setValue(element, value)
        });

    this.When(/^I select value "([^"]*)?" from element "([^"]*)?"$/, (value, element) => {
            browser.selectByValue(element,value)
        });

    this.When(/^I click on the key "([^"]*)?"$/, (value) => {
            browser.keys(value)
        });

    this.When(/^I go back to last page$/, () => {
            browser.back()
        });
    
    this.When(/^I click on keys "([^"]*)?"$/, (value) => {
            browser.keys(value)
        });

    this.When(/^I wait for the element "([^"]*)?"$/, (value) => {
            browser.waitForEnabled(value)
        });

    this.When(/^I sign out$/, () => {
            browser.click('span=Profile');
            browser.click('button=Sign out');
        });

    this.When(/^I sign up as "([^"]*)?"$/, (email) => {
            browser
            .click('a=Sign up')
            .setValue('#FirstName', 'firstName')
            .setValue('#LastName', 'lastName')
            .setValue('#Email', email)
            .setValue('#Password', 'Test12345')
            .setValue('#ConfirmPassword', 'Test12345')
            .click('span=I agree to')
            .click('button=Create an account')
        });

    this.When(/^I sign in as "([^"]*)?" with password "([^"]*)?"$/, (email, password) => {
            browser
            .click('span=Log in')
            .setValue('#Email', email)
            .setValue('#Password', password)
            .click('button=Sign in')
        });

    this.When(/^I sign in as user$/, () => {
            browser
            .click('span=Log in')
            .setValue('#Email', 'test@agileworks.eu')
            .setValue('#Password', 'ChangeMe123!')
            .click('button=Sign in')
        });

    this.When(/^I sign in as mentor$/, () => {
            browser
            .click('span=Log in')
            .setValue('#Email', 'mentor@agileworks.eu')
            .setValue('#Password', 'ChangeMe123!')
            .click('button=Sign in')
        });

    this.When(/^I sign in as administrator$/, () => {
            browser
            .click('span=Log in')
            .setValue('#Email', 'admin@agileworks.eu')
            .setValue('#Password', 'ChangeMe123!')
            .click('button=Sign in')
        });

    this.When(/^I answer ([^\s]+) to question "([^"]*)?"$/, (answer, question) => {
        expect(browser.isVisibleWithinViewport("h1=" + question))
        if (answer.toLowerCase() === "yes") {
            browser.click("button=Yes")  
        } else if (answer.toLowerCase() === "no") {
            browser.click("button=No")  
        } else {
            throw "Answer could not be parsed: " + answer
        }
    });
}