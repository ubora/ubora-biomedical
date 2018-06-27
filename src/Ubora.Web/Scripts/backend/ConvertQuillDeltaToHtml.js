// https://github.com/joelcolucci/node-quill-converter/blob/master/lib/index.js
module.exports = function (callback, deltaString) {
    const fs = require('fs');
    const path = require('path');

    const { JSDOM } = require('jsdom');
    const { Script } = require("vm");

    let quillFilePath = require.resolve('quill');
    let quillMinFilePath = quillFilePath.replace('quill.js', 'quill.min.js');

    let quillLibrary = fs.readFileSync(quillMinFilePath);
    let mutationObserverPolyfill = fs.readFileSync(path.join(__dirname, '_polyfill.js'));

    const JSDOM_TEMPLATE = `
  <div id="editor"></div>
  <script>${mutationObserverPolyfill}</script>
  <script>${quillLibrary}</script>
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

    const JSDOM_OPTIONS = { runScripts: 'dangerously', resources: 'usable' };

    const DOM = new JSDOM(JSDOM_TEMPLATE, JSDOM_OPTIONS);
    const WINDOW = DOM.window;
    const DOCUMENT = WINDOW.document;

    const QUILL = new DOM.window.Quill('#editor');
    QUILL.setContents(JSON.parse(deltaString), 'api');

    const quillHtml = DOCUMENT.querySelector(".ql-editor").innerHTML;

    callback(null, quillHtml);
};