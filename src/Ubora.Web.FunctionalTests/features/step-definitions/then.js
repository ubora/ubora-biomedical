const expect = require('chai').expect;

module.exports = function () {
    this.Then(/^I expect the title of the page "([^"]*)"$/, (title) => {
        expect(browser.getTitle()).to.be.eql(title);
    });

    this.Then(/^I expect that should be "([^"]*)?" the text of the page "([^"]*)"$/, (text, element) => {
        expect(browser.getText(element)).to.be.eql(text);
    });

    this.Then(/^I expect that the element "([^"]*)?" is visible$/, (element) => {
        expect(browser.isVisible(element));
    });
}