global.UBORA.initAddOrEditResource = function (editorSelector, toolbarSelector, titleInputSelector, contentInputSelector, exampleTitle, initialContent) {
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
    
    var titleInput = $(titleInputSelector);
    if (titleInput.val()) {
        Slugify(titleInput.val());
    } else {
        Slugify(exampleTitle);
    }

    titleInput.change(function () {
        const textToSlugify = titleInput.val() || exampleTitle;
        Slugify(textToSlugify);
    });

    function Slugify(text) {
        $.ajax({
                url: "/resources/slugify",
                data: { text: text }
            })
            .done((slug) => {
                $("#js-slug").text(slug);
            })
            .fail(() => {
                $("#js-slug").text("error generating URL...");
            });
    }

    quill.on('editor-change', function () {
        $(contentInputSelector).val(JSON.stringify(quill.getContents()));
    });
};