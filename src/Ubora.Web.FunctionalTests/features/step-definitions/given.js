const expect = require('chai').expect;

module.exports = function () {
    this.Given(/^I clicked on the element "([^"]*)?"$/, (element) => {
        browser.waitForVisible(element, 1500);
        browser.click(element);
    });

    this.Given(/^I clicked on the element "([^"]*)?" inside "([^"]*)?"$/, (element, insideElement) => {
        browser.element(insideElement).waitForVisible(element, 1500);
        browser.element(insideElement).click(element);
    });

    this.Given(/^I go to Home page$/, () => {
        browser.url('/')
    });

    this.Given(/^I signed in as user$/, () => {
        browser
        .click('span=Log in')
        .setValue('#Email', 'test@agileworks.eu')
        .setValue('#Password', 'ChangeMe123!')
        .click('button=Log in')
    });

    this.Given(/^I am signed in as administrator on first page$/, () => {
        browser
        .deleteCookie(".AspNetCore.Identity.Application")
        .url('/')
        .click('span=Log in');
        browser.waitForEnabled('#Email');
        browser.setValue('#Email', 'admin@agileworks.eu')
        .setValue('#Password', 'ChangeMe123!')
        .click('button=Log in')
    });

    this.Given(/^I am signed in as user and on first page$/, () => {
        browser
        .deleteCookie(".AspNetCore.Identity.Application")
        .url('/')
        .click('span=Log in');
        browser.waitForEnabled('#Email');
        browser.setValue('#Email', 'test@agileworks.eu')
        .setValue('#Password', 'ChangeMe123!')
        .click('button=Log in')
    });

    this.Given(/^I expected the element "([^"]*)?" is visible$/, (element) => {
        expect(browser.isVisible(element))
    });

    this.Given(/^I expected the title of the page "([^"]*)"$/, (title) => {
        expect(browser.getTitle()).to.be.eql(title)
    });

    this.Given(/^I sign up as "([^"]*)?" and on first page$/, (email) => {
        browser
        .deleteCookie('.AspNetCore.Identity.Application')
        .url('/')
        .click('span=Log in')
        .click('a=Sign up')
        .setValue('#FirstName', 'testFirstName')
        .setValue('#LastName', 'testLastName')
        .setValue('#Email', email)
        .setValue('#Password', 'Test12345')
        .setValue('#ConfirmPassword', 'Test12345')
        .click('span=I agree to')
        .click('button=Create an account')
        .url('/')
    });
}