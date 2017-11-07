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

    this.Then(/^I expect the question "([^"]*)?" is visible$/, (question) => {
        expect(browser.isVisibleWithinViewport("h1=" + question))
    });
}