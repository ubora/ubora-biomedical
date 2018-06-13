global.UBORA.initAddResource = function (titleInputSelector, contentInputSelector, exampleTitle) {
    var quill = new Quill('#editor-container',
        {
            theme: 'snow'
        });

    $('#editor-container').show();

    Slugify(exampleTitle);

    var titleInput = $(titleInputSelector);
    titleInput.change(function () {
        const textToSlugify = titleInput.val() || exampleTitle;
        Slugify(textToSlugify);
    });

    function Slugify(text) {
        $.ajax({
                url: "slugify",
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