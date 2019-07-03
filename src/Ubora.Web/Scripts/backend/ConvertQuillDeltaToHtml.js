// https://github.com/joelcolucci/node-quill-converter/blob/master/lib/index.js
module.exports = function (callback, deltaString) {
    var QuillDeltaToHtmlConverter = require('quill-delta-to-html');

    var deltaOps = JSON.parse(deltaString).ops;
    var cfg = {};
    var converter = new QuillDeltaToHtmlConverter(deltaOps, cfg);
    var html = converter.convert(); 

    callback(null, html);
};