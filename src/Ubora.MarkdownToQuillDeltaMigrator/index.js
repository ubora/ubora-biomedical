module.exports = function (callback, markdown) {
    const marked = require('marked');
    const { convertHtmlToDelta } = require('./node-quill-converter.js');

    const renderer = new marked.Renderer();
    renderer.link = (href, title, text) => {
        const linkedDocument = `<a target="_blank" href="${href}" title="${title}">${text}</a>`;
        return linkedDocument;
    };
    renderer.image = (href, title) => {
        const optimizedImage = `<img class="image-markdown" src="${href}" alt="${title}"`;
        return optimizedImage;
    };

    const markdownRendererOptions = {
        renderer: renderer,
        gfm: true,
        tables: true,
        breaks: true,
        pedantic: false,
        sanitize: true,
        smartLists: true,
        smartypants: false
    };

    marked.setOptions(markdownRendererOptions);
    var html = marked(markdown, { renderer: this.renderer });

    var delta = convertHtmlToDelta(html);

    callback(null, JSON.stringify(delta));
}