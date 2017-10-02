const expect = require('chai').expect;

module.exports = function () {
    this.Given(/^I clicked on the element "([^"]*)?"$/, (element) => {
            browser.click(element)
        });

    this.Given(/^I go to Home page$/, () => {
        browser.url('/')
    });

    this.Given(/^I signed in as user$/, () => {
        browser
        .click('#SignInSignUp')
        .setValue('#Email', 'test@agileworks.eu')
        .setValue('#Password', 'ChangeMe123!')
        .click('button=Sign in')
    });

    this.Given(/^I am signed in as user and on first page$/, () => {
        browser
        .deleteCookie(".AspNetCore.Identity.Application")
        .url('/')
        .click('#SignInSignUp')
        .setValue('#Email', 'test@agileworks.eu')
        .setValue('#Password', 'ChangeMe123!')
        .click('button=Sign in')
    });

    this.Given(/^I expected the element "([^"]*)?" is visible$/, (element) => {
        expect(browser.isVisible(element))
    });

    this.Given(/^I expected the title of the page "([^"]*)"$/, (title) => {
        expect(browser.getTitle()).to.be.eql(title)
    });
}