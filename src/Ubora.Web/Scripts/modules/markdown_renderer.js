class MarkdownRenderer {
  constructor(element) {
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

    this.renderMarkdown(element);
  }

  renderMarkdown(element) {
    marked(element);
  }
}

const markdownSelector = '.text-markdown';
const markdownElementCollection = document.querySelectorAll(markdownSelector);

if (markdownElementCollection.length > 0) {
  const markdownRenderer = new MarkdownRenderer();
  Array.prototype.map.call(markdownElementCollection, markdownElement => {
    return markdownRenderer.renderMarkdown(markdownElement);
  });
}

// TODO: Risto Kitsing - https://github.com/chjj/marked
// const markdownContainerArray = document.querySelectorAll('.text-markdown');
// const markdownArrayHasItems = markdownContainerArray.length > 0;
//
// if (markdownArrayHasItems) {
//   for (let i = 0; i < markdownContainerArray.length; i++) {
//     let innerHtmlUnencoded = markdownContainerArray[i].innerHTML.replace(/&gt;/g, '>');
//     markdownContainerArray[i].innerHTML = marked(innerHtmlUnencoded, { renderer: renderer });
//   }
// }
