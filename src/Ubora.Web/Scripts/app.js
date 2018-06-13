import MarkdownRenderer from './modules/markdown_renderer';
import MarkdownEditor from './modules/markdown_editor';
import Autocomplete from './modules/autocomplete';
import Feedback from './modules/feedback';

import '../Styles/styles';

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover();
});

$(function () {
    const clipboard = new ClipboardJS('.js-clipboard');

    clipboard.on('success', function (e) {
        showMomentaryTooltipMessage(e, 'Copied!');
        e.clearSelection();
    });

    clipboard.on('error', function (e) {
        showMomentaryTooltipMessage(e, 'Press Ctrl+C to copy');
        e.clearSelection();
    });

    function showMomentaryTooltipMessage(e, message) {
        const $trigger = $(e.trigger);
        const initialTitle = $trigger.attr('data-original-title');
        $trigger.attr('data-original-title', message).tooltip('show');
        $trigger.attr('data-original-title', initialTitle);
    }
});

$(function () {
    $('form').on('submit', function () {
        if ($(this).valid()) {
            $(this).find('button[type=submit]').prop('disabled', true);
        }
    });
});

global.UBORA = {};

global.UBORA.slugify = text => {
    // Use hash map for special characters 
    let specialChars = { "à": 'a', "ä": 'a', "á": 'a', "â": 'a', "æ": 'a', "å": 'a', "ë": 'e', "è": 'e', "é": 'e', "ê": 'e', "î": 'i', "ï": 'i', "ì": 'i', "í": 'i', "ò": 'o', "ó": 'o', "ö": 'o', "ô": 'o', "ø": 'o', "ù": 'o', "ú": 'u', "ü": 'u', "û": 'u', "ñ": 'n', "ç": 'c', "ß": 's', "ÿ": 'y', "œ": 'o', "ŕ": 'r', "ś": 's', "ń": 'n', "ṕ": 'p', "ẃ": 'w', "ǵ": 'g', "ǹ": 'n', "ḿ": 'm', "ǘ": 'u', "ẍ": 'x', "ź": 'z', "ḧ": 'h', "·": '-', "/": '-', "_": '-', ",": '-', ":": '-', ";": '-' };

    return text.toString().toLowerCase()
        .replace(/\s+/g, '-')           // Replace spaces with -
        .replace(/./g, (target, index, str) => specialChars[target] || target) // Replace special characters using the hash map
        .replace(/&/g, '-and-')         // Replace & with 'and'
        .replace(/[^\w\-]+/g, '')       // Remove all non-word chars
        .replace(/\-\-+/g, '-')         // Replace multiple - with single -
        .replace(/^-+/, '')             // Trim - from start of text
        .replace(/-+$/, '');             // Trim - from end of text
};