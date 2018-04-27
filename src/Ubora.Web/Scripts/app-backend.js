const QuillDeltaToHtmlConverter = require("quill-delta-to-html");
const document = require("min-document");

const convertQuillDeltaToHtml = function (callback, delta) {
    const config = {
        encodeHtml: true
    };
    const deltaOps = JSON.parse(delta).ops;
    const converter = new QuillDeltaToHtmlConverter(deltaOps, config);
    const html = converter.convert();

    callback(null, html);
};

module.exports = {
    convertQuillDeltaToHtml
};