'use strict';

const styles = require('../Styles/styles.css').toString();

if (module.hot) {
  module.hot.accept();
}

const sidemenu = require('./modules/sidemenu');
const marked = require('./modules/marked');
const autocomplete = require('./modules/autocomplete');
const notices = require('./modules/notices');
const feedback = require('./modules/feedback');

window.addEventListener('load', () => {
  console.info(`UBORA: page loaded in ${Math.ceil(window.performance.now())}ms / target <500ms`);
});

// console.info('@Context.Request.Path', '@Url.Action("Send", "Feedback")');
// // page slug + /Feedback/Send
