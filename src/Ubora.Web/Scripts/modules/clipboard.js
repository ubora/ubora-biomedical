export class CopyingToClipboard {
    static initialize() {

        function copyToClipboard(text, copyElement) {
            const copyTest = document.queryCommandSupported('copy');
            const elOriginalText = copyElement.attr('data-original-title');

            if (copyTest === true) {
                const copyTextArea = document.createElement("textarea");
                copyTextArea.value = text;
                document.body.appendChild(copyTextArea);
                copyTextArea.select();
                try {
                    const successful = document.execCommand('copy');
                    const msg = successful ? 'Copied!' : 'Whoops, not copied!';
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
                const $this = $(this);

                const text = $this.attr('data-copy');
                const copyElement = $this;
                copyToClipboard(text, copyElement);
            });
        });
    }
}

CopyingToClipboard.initialize();
