const expect = require('chai').expect;

module.exports = function () {
    this.When(/^I click on the element "([^"]*)?"$/, (element) => {
            browser.waitForVisible(element, 1500);
            browser.click(element);
        });

    this.When(/^I click on the href element "([^"]*)?"$/, (partialHref) => {
            browser.click('[href*="' + partialHref + '"]');
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
            browser.waitForVisible(value)
        });

    this.When(/^I sign out$/, () => {
            browser.click('span=Log out');
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
            .click('button=Log in')
        });

    this.When(/^I sign in as user$/, () => {
            browser
            .click('span=Log in')
            .setValue('#Email', 'test@agileworks.eu')
            .setValue('#Password', 'ChangeMe123!')
            .click('button=Log in')
        });

    this.When(/^I sign in as mentor$/, () => {
            browser
            .click('span=Log in')
            .setValue('#Email', 'mentor@agileworks.eu')
            .setValue('#Password', 'ChangeMe123!')
            .click('button=Log in')
        });

    this.When(/^I sign in as administrator$/, () => {
            browser
            .click('span=Log in')
            .setValue('#Email', 'admin@agileworks.eu')
            .setValue('#Password', 'ChangeMe123!')
            .click('button=Log in')
        });

    this.When(/^I answer ([^\s]+) to question "([^"]*)?"$/, (answer, question) => {
        var isVisible = browser.isVisible("h5*=" + question);
        expect(isVisible).to.equal(true, `Expected "${"h5=" + question}" to be visible.`);
        if (answer.toLowerCase() === "yes") {
            browser.click("button=Yes");
        } else if (answer.toLowerCase() === "no") {
            browser.click("button=No");
        } else {
            throw "Answer could not be parsed: " + answer
        }
    });

    this.When(/^I answer "([^"]*)?" to the question "([^"]*)?"$/, (answer, question) => {
        var isVisible = browser.isVisible("h5*=" + question);
        expect(isVisible).to.equal(true, `Expected "${"h5=" + question}" to be visible.`);
        const answerElements = browser.elements("label=" + answer);
        if (answerElements.value.length > 1) {
            var lastElement = answerElements.value.slice(-1)[0];
            lastElement.click();
        } else {
            browser.element("label=" + answer).click();
        }
        browser.click("button=Answer")
    });

    this.When(/^I answer ([^\s]+) to test question "([^"]*)?"$/, (answer, question) => {
        var isVisible = browser.isVisible(question);
        expect(isVisible).to.equal(true, `Expected "${"test=" + question}" to be visible.`);
        if (answer.toLowerCase() === "yes") {
            browser.click("button=Yes");
        } else if (answer.toLowerCase() === "no") {
            browser.click("button=No");
        } else {
            throw "Answer could not be parsed: " + answer
        }
    });

    this.When(/^I answer "([^"]*)?" to the test question "([^"]*)?"$/, (answer, question) => {

        var isVisible = browser.isVisible(question);
        expect(isVisible).to.equal(true, `Expected "${"test=" + question}" to be visible.`);
        const answerElements = browser.elements("label=" + answer);
        if (answerElements.value.length > 1) {
            var lastElement = answerElements.value.slice(-1)[0];
            lastElement.click();
        } else {
            browser.element("label=" + answer).click();
        }
        browser.click("button=Answer")
    });

}