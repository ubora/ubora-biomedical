global.UBORA.initAddOrEditResource = function (editorSelector, toolbarSelector, contentInputSelector, initialContent) {
    var quill = new Quill(editorSelector,
        {
            theme: 'snow',
            modules: {
                toolbar: toolbarSelector
            }
        });

    if (!!initialContent) {
        quill.setContents(initialContent, 'api');
    }

    $(editorSelector).show();
    $(toolbarSelector).show();

    quill.getModule('toolbar').addHandler('image', () => {
        imageHandler();
    });

    function imageHandler() {
        var range = quill.getSelection();
        var value = prompt('What is the image URL');
        quill.insertEmbed(range.index, 'image', value, Quill.sources.USER);
    }

    quill.on('editor-change', function () {
        $(contentInputSelector).val(JSON.stringify(quill.getContents())).trigger('change');
    });
};