'use strict';

const styles = require('../Styles/styles.css').toString();

if (module.hot) {
  module.hot.accept();
}

const sidemenu = require('./modules/sidemenu');
const autocomplete = require('./modules/autocomplete');
const notices = require('./modules/notices');
const feedback = require('./modules/feedback');

const markdown_editor = require('./modules/markdown_editor');
const markdown_renderer = require('./modules/markdown_renderer');

window.addEventListener('load', () => {
  console.info(`UBORA: page loaded in ${Math.ceil(window.performance.now())}ms / target <500ms`);
});

// console.info('@Context.Request.Path', '@Url.Action("Send", "Feedback")');
// // page slug + /Feedback/Send
