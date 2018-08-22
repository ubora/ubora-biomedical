global.UBORA.initEditor = function (editorSelector, toolbarSelector, contentInputSelector, initialContent) {
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

    quill.on('editor-change', function () {
        $(contentInputSelector).val(JSON.stringify(quill.getContents())).trigger('change');
    });
};