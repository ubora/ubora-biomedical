const expect = require('chai').expect;

module.exports = function () {
    this.Then(/^I expect the title of the page "([^"]*)"$/, (title) => {
        browser.waitUntil(function () {
            return browser.getTitle() === title;
        }, 5000);
        expect(browser.getTitle()).to.be.eql(title)
    });

    this.Then(/^I expect the element "([^"]*)?" is visible$/, (element) => {
        browser.waitForVisible(element, 1500);
        var isVisible = browser.isVisible(element);

        if (isVisible instanceof Array) {
            isVisible = isVisible[0];
        }
        expect(isVisible).to.equal(true, `Expected "${element}" to be visible.`);
    });

    this.Then(/^I expect the element "([^"]*)?" inside "([^"]*)?" is visible$/, (element, insideElement) => {
        browser.element(insideElement).waitForVisible(element, 1500);
        var isVisible = browser.element(insideElement).isVisible(element);

        if (isVisible instanceof Array) {
            isVisible = isVisible[0];
        }
        expect(isVisible).to.equal(true, `Expected "${element}" to be visible.`);
    });

    this.Then(/^I expect the element "([^"]*)?" is not visible$/, (element) => {
        var isVisible = browser.isVisible(element);

        if (isVisible instanceof Array) {
            isVisible = isVisible[0];
        }
        expect(isVisible).to.equal(false, `Expected "${element}" to not be visible.`);
    });

    this.Then(/^I expect the element "([^"]*)?" to contain text "([^"]*)?"$/, (element, expectedText) => {
        var actualText = browser.getValue(element);
        expect(actualText).to.contain(expectedText)
    });

    this.Then(/^I expect the console output to clear$/, () => {
        var javascriptLogs = browser.log('browser').value.filter(function(element) {
            return element.source === 'javascript'
        });

        if(javascriptLogs.lenght !== null && javascriptLogs.lenght){
            throw new Error("Exception: " + JSON.stringify(javascriptLogs));
        }

        expect(javascriptLogs).to.have.lengthOf(0);
    });
}