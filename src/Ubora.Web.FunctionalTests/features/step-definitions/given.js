const expect = require('chai').expect;

module.exports = function () {
    this.Given(/^I go to the website "([^"]*)"$/, (url) => {
        const newUrl = url;
        browser.url(newUrl);
    });
    
}