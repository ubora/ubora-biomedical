class MarkdownEditor {
  constructor() {
    if (window.marked) {
      const renderer = new marked.Renderer();

      renderer.link = (href, title, text) => {
        const linkedDocument = `<a target="_blank" href="${href}" title="${title}">${text}</a>`;
        return linkedDocument;
      };

      renderer.image = (href, title) => {
        const optimizedImage = `<img class="image-markdown" src="${href}" alt="${title}"`;
        return optimizedImage;
      };

      this._loadMarkdownOptions();
    }
  }

  _loadMarkdownOptions() {
    const options = {
      renderer: renderer,
      gfm: true,
      tables: true,
      breaks: true,
      pedantic: false,
      sanitize: true,
      smartLists: true,
      smartypants: false
    };

    return marked.setOptions(options);
  }

}
  //
  // if (window.marked) {
  //   const renderer = new marked.Renderer();
  //
  //
  //
  //
  //
  //   const markdownContainerArray = document.querySelectorAll('.text-markdown');
  //   const markdownArrayHasItems = markdownContainerArray.length > 0;
  //
  //   if (markdownArrayHasItems) {
  //     for (let i = 0; i < markdownContainerArray.length; i++) {
  //         let innerHtmlUnencoded = markdownContainerArray[i].innerHTML.replace(/&gt;/g, '>');
  //         markdownContainerArray[i].innerHTML = marked(innerHtmlUnencoded, { renderer: renderer });
  //       }
  //   }
  //
  //   const simpleMde = new SimpleMDE({
  //       element: document.querySelector('.content_editable'), previewRender: plainText => {
  //           return marked(plainText, { renderer: renderer });
  //       },
  //       toolbar: [{
  //           name: "bold",
  //           action: SimpleMDE.toggleBold,
  //           className: "fa fa-bold",
  //           title: "Bold"
  //       },
  //       {
  //           name: "italic",
  //           action: SimpleMDE.toggleItalic,
  //           className: "fa fa-italic",
  //           title: "Italic",
  //           default: true
  //       },
  //       {
  //           name: "heading",
  //           action: SimpleMDE.toggleHeadingSmaller,
  //           className: "fa fa-header",
  //           title: "Heading",
  //           default: true
  //       },
  //
  //           "|",
  //       {
  //           name: "quote",
  //           action: SimpleMDE.toggleBlockquote,
  //           className: "fa fa-quote-left",
  //           title: "Quote",
  //           default: true
  //       },
  //       {
  //           name: "unordered-list",
  //           action: SimpleMDE.toggleUnorderedList,
  //           className: "fa fa-list-ul",
  //           title: "Generic List",
  //           default: true
  //       },
  //       {
  //           name: "ordered-list",
  //           action: SimpleMDE.toggleOrderedList,
  //           className: "fa fa-list-ol",
  //           title: "Numbered List",
  //           default: true
  //       },
  //           "|",
  //       {
  //           name: "link",
  //           action: SimpleMDE.drawLink,
  //           className: "fa fa-link",
  //           title: "Create Link",
  //           default: true
  //       },
  //       {
  //           name: "image",
  //           action: SimpleMDE.drawImage,
  //           className: "fa fa-picture-o",
  //           title: "Insert Image",
  //           default: true
  //       },
  //           "|",
  //       {
  //           name: "preview",
  //           action: SimpleMDE.togglePreview,
  //           className: "fa fa-eye no-disable",
  //           title: "Toggle Preview",
  //           default: true
  //       },
  //       {
  //           name: "fullscreen",
  //           action: SimpleMDE.toggleFullScreen,
  //           className: "fa fa-arrows-alt no-disable no-mobile",
  //           title: "Toggle Fullscreen",
  //           default: true
  //       },
  //           "|",
  //       {
  //           name: "guide",
  //           action: "//simplemde.com/markdown-guide",
  //           className: "fa fa-question-circle",
  //           title: "Markdown Guide",
  //           default: true
  //       },
  //           "|",
  //       {
  //           name: "undo",
  //           action: SimpleMDE.undo,
  //           className: "fa fa-undo no-disable",
  //           title: "Undo"
  //       },
  //       {
  //           name: "redo",
  //           action: SimpleMDE.redo,
  //           className: "fa fa-repeat no-disable",
  //           title: "Redo"
  //       }
  //       ]
  //   });
  // }
