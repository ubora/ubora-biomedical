import MarkdownRenderer from './modules/markdown_renderer';
import MarkdownEditor from './modules/markdown_editor';
import Autocomplete from './modules/autocomplete';
import Feedback from './modules/feedback';

import '../Styles/styles';

$(function () {
    // Bootstrap tooltips/popovers
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover();

    // Clipboard
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

    // Disable form button when submitted & and don't ask confirmation about navigating away
    $('form').on('submit', function () {
        const $this = $(this);
        if ($this.valid()) {
            window.onbeforeunload = null;
            $this.find('button[type=submit]').prop('disabled', true);
        }
    });

    var possiblyChangedForms = [];

    // Ask whether the user wants to navigate away
    $('form[method="post"]')
        .each(function (i, element) {
            const $form = $(element);

            if (!$form.hasClass('js-onbeforeunload')) {
                return;
            };

            console.log('foo')

            possiblyChangedForms.push({ form: $form, initialSerialize: $form.serialize() })
        })
        .on('change', function () {
            window.onbeforeunload = null;

            possiblyChangedForms.forEach(function (item) {
                console.log(item.form.serialize())

                if (item.form.serialize() !== item.initialSerialize) {
                    window.onbeforeunload = function () { return "You may have unsaved changes. Do you really want to leave?" };
                    return;
                }
            });
        })
});

global.UBORA = {};