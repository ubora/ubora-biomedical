import MarkdownRenderer from './modules/markdown_renderer';
import MarkdownEditor from './modules/markdown_editor';
import Autocomplete from './modules/autocomplete';
import Feedback from './modules/feedback';
import ClipboardJS from 'clipboard';

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
    window.clickAndDisable = function (element) {
        element.onclick = function (event) {
            event.preventDefault();
        }
    } 
});

global.UBORA = {};
