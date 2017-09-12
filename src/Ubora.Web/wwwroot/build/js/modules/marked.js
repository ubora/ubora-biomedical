var renderer = new marked.Renderer();

marked.setOptions({
    renderer: renderer,
    gfm: true,
    tables: true,
    breaks: true,
    pedantic: false,
    sanitize: true,
    smartLists: true,
    smartypants: false
});

// Open link which redirects out of Ubora in new tab/window.
renderer.link = function (href, title, text) {
    return '<a target="_blank" href="' + href + '" title="' + title + '">' + text + '</a>';
};

// Picture size is optimized.
renderer.image = function (href, title) {
    return '<img class="image-markdown" src="' + href + ' " alt="' + title + '" />';
};

var markdownContainerArray = document.querySelectorAll('.text-markdown');
var markdownArrayHasItems = markdownContainerArray.length > 0;

if (markdownArrayHasItems) {
    for (var i = 0; i < markdownContainerArray.length; i++) {

        // .innerHtml, .text etc always get encoded unless using <textarea>. Because we need '>' for blockquotes then unencode it.
        var innerHtmlUnencoded = markdownContainerArray[i].innerHTML
            .replace(/&gt;/g, '>'); // Using regex will replace all occurrences.

        markdownContainerArray[i].innerHTML = marked(innerHtmlUnencoded, { renderer: renderer });
    }
}

var simpleMde = new SimpleMDE({
    element: document.querySelector('.content_editable'), previewRender: function (plainText) {
        return marked(plainText, { renderer: renderer });
    },
    toolbar: [{
        name: "bold",
        action: SimpleMDE.toggleBold,
        className: "fa fa-bold",
        title: "Bold"
    },
    {
        name: "italic",
        action: SimpleMDE.toggleItalic,
        className: "fa fa-italic",
        title: "Italic",
        default: true
    },
    {
        name: "heading",
        action: SimpleMDE.toggleHeadingSmaller,
        className: "fa fa-header",
        title: "Heading",
        default: true
    },

        "|",
    {
        name: "quote",
        action: SimpleMDE.toggleBlockquote,
        className: "fa fa-quote-left",
        title: "Quote",
        default: true
    },
    {
        name: "unordered-list",
        action: SimpleMDE.toggleUnorderedList,
        className: "fa fa-list-ul",
        title: "Generic List",
        default: true
    },
    {
        name: "ordered-list",
        action: SimpleMDE.toggleOrderedList,
        className: "fa fa-list-ol",
        title: "Numbered List",
        default: true
    },
        "|",
    {
        name: "link",
        action: SimpleMDE.drawLink,
        className: "fa fa-link",
        title: "Create Link",
        default: true
    },
    {
        name: "image",
        action: SimpleMDE.drawImage,
        className: "fa fa-picture-o",
        title: "Insert Image",
        default: true
    },
        "|",
    {
        name: "preview",
        action: SimpleMDE.togglePreview,
        className: "fa fa-eye no-disable",
        title: "Toggle Preview",
        default: true
    },
    {
        name: "fullscreen",
        action: SimpleMDE.toggleFullScreen,
        className: "fa fa-arrows-alt no-disable no-mobile",
        title: "Toggle Fullscreen",
        default: true
    },
        "|",
    {
        name: "guide",
        action: "https://simplemde.com/markdown-guide",
        className: "fa fa-question-circle",
        title: "Markdown Guide",
        default: true
    },
        "|",
    {
        name: "undo",
        action: SimpleMDE.undo,
        className: "fa fa-undo no-disable",
        title: "Undo"
    },
    {
        name: "redo",
        action: SimpleMDE.redo,
        className: "fa fa-repeat no-disable",
        title: "Redo"
    }
    ]
});
