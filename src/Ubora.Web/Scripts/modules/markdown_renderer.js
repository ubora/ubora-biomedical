export class MarkdownRenderer {
  constructor() {
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
  }

  renderMarkdown(element) {
    const UnEncodedInnerHtml = element.innerHTML.replace(/&gt;/g, '>');
    return element.innerHTML = window.marked(UnEncodedInnerHtml, { renderer: this.renderer });
  }
}

const markdownSelector = '.text-markdown';
const markdownElementCollection = document.querySelectorAll(markdownSelector);

if (markdownElementCollection.length > 0 && window.marked) {
  const markdownRenderer = new MarkdownRenderer();
  Array.prototype.map.call(markdownElementCollection, markdownElement => {
    return markdownRenderer.renderMarkdown(markdownElement);
  });
}
