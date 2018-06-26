const fs = require('fs');
const path = require('path');

const { JSDOM } = require('jsdom');
const { Script } = require("vm");

// https://github.com/joelcolucci/node-quill-converter/blob/master/lib/index.js
const convertQuillDeltaToHtml = function (callback, deltaString) {
    let quillFilePath = require.resolve('quill');
    let quillMinFilePath = quillFilePath.replace('quill.js', 'quill.min.js');

    let jqueryFilePath = require.resolve('jQuery');
    let jqueryMinFilePath = jqueryFilePath.replace('jquery.js', 'jquery.min.js');

    let quillLibrary = fs.readFileSync(quillMinFilePath);
    let mutationObserverPolyfill = fs.readFileSync(path.join(__dirname, 'polyfill.js'));
    let jqueryLibrary = fs.readFileSync(jqueryMinFilePath);

    const JSDOM_TEMPLATE = `
  <div id="editor"></div>
  <div id="table-of-contents"></div>
  <script>${mutationObserverPolyfill}</script>
  <script>${quillLibrary}</script>
  <script>${jqueryLibrary}</script>
  <script>
    document.getSelection = function() {
      return {
        getRangeAt: function() { }
      };
    };
    document.execCommand = function (command, showUI, value) {
      try {
          return document.execCommand(command, showUI, value);
      } catch(e) {}
      return false;
    };
  </script>
`;

    const JSDOM_OPTIONS = { runScripts: 'dangerously', resources: 'usable'};

    const DOM = new JSDOM(JSDOM_TEMPLATE, JSDOM_OPTIONS);
    const WINDOW = DOM.window;
    const DOCUMENT = WINDOW.document;

    const QUILL = new DOM.window.Quill('#editor');
    QUILL.setContents(JSON.parse(deltaString), 'api');

    const addHeadingIdsScript = new Script(fs.readFileSync(path.join(__dirname, '_add-heading-ids.js')));
    DOM.runVMScript(addHeadingIdsScript);

    const quillHtml = DOCUMENT.querySelector(".ql-editor").innerHTML;

    callback(null, quillHtml);
};

module.exports = {
    convertQuillDeltaToHtml
};