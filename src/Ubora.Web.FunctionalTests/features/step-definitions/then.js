const expect = require('chai').expect;

module.exports = function () {
    this.Then(/^I expect the title of the page "([^"]*)"$/, (title) => {
        expect(browser.getTitle()).to.be.eql(title)
    });

    this.Then(/^I expect the element "([^"]*)?" is visible$/, (element) => {
        expect(browser.isVisible(element))
    });
}