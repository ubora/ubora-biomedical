const expect = require('chai').expect;

module.exports = function () {
    // Put 'Debug' anywhere to stop execution at that point. Read more: http://webdriver.io/api/utility/debug.html
    this.Then(/^Debug$/, () => {
        browser.debug()
    });
}