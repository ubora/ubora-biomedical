export class CopyingToClipboard {
    constructor() {

        function copyToClipboard(text, copyElement) {
            var copyTest = document.queryCommandSupported('copy');
            var elOriginalText = copyElement.attr('data-original-title');

            if (copyTest === true) {
                var copyTextArea = document.createElement("textarea");
                copyTextArea.value = text;
                document.body.appendChild(copyTextArea);
                copyTextArea.select();
                try {
                    var successful = document.execCommand('copy');
                    var msg = successful ? 'Copied!' : 'Whoops, not copied!';
                    copyElement.attr('data-original-title', msg).tooltip('show');
                } catch (err) {
                    console.log('Oops, unable to copy');
                }
                document.body.removeChild(copyTextArea);
                copyElement.attr('data-original-title', elOriginalText);
            } else {
                // Fallback if browser doesn't support .execCommand('copy')
                window.prompt("Copy to clipboard: Ctrl+C or Command+C, Enter", text);
            }
        }

        $(document).ready(function () {

            // Tooltip
            $('.js-tooltip').tooltip();

            // Copy to clipboard
            $('.js-copy').click(function () {
                var text = $(this).attr('data-copy');
                var copyElement = $(this);
                copyToClipboard(text, copyElement);
            });
        });
    }
}

new CopyingToClipboard();
