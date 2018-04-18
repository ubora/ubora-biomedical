const expect = require('chai').expect;

module.exports = function () {
    this.Then(/^I expect the title of the page "([^"]*)"$/, (title) => {
        expect(browser.getTitle()).to.be.eql(title)
    });

    this.Then(/^I expect the element "([^"]*)?" is visible$/, (element) => {
        browser.waitForVisible(element, 1500);
        var isVisible = browser.isVisible(element);
        expect(isVisible).to.equal(true, `Expected "${element}" to be visible.`);
    });

    this.Then(/^I expect the element "([^"]*)?" inside "([^"]*)?" is visible$/, (element, insideElement) => {
        browser.element(insideElement).waitForVisible(element, 1500);
        var isVisible = browser.element(insideElement).isVisible(element);
        expect(isVisible).to.equal(true, `Expected "${element}" to be visible.`);
    });

    this.Then(/^I expect the element "([^"]*)?" is not visible$/, (element) => {
        var isVisible = browser.isVisible(element);
        expect(isVisible).to.equal(false, `Expected "${element}" to be not visible.`);
    });

    this.Then(/^I expect the element "([^"]*)?" to contain text "([^"]*)?"$/, (element, expectedText) => {
        var actualText = browser.getValue(element);
        expect(actualText).to.contain(expectedText)
    });
}