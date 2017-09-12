'use strict';

window.addEventListener('load', () => {
  console.info(`UBORA: page loaded in ${Math.ceil(window.performance.now())}ms / target <500ms`);
});

if (module.hot) {
  module.hot.accept();
}

import css from '../css/styles.css';

const ubora_autocomplete = require('./modules/autocomplete.js');
const ubora_feedback = require('./modules/feedback.js');
const ubora_marked = require('./modules/marked.js');
const ubora_notices = require('./modules/notices.js');
const ubora_sidemenu = require('./modules/sidemenu.js');
