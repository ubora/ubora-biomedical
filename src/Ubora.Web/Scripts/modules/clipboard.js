export class CopyToClipboard {
  constructor() {
    const clipboardTriggerNodeList = document.querySelectorAll('.js-copy-to-clipboard');

    if (clipboardTriggerNodeList.length > 0) {
      Array.prototype.map.call(clipboardTriggerNodeList, clipboardTriggerElement => {
        clipboardTriggerElement.addEventListener('click', event => {
          const bodyElement = document.querySelector('body');
          const uriPath = event.target.getAttribute('data-uripath');
          const textareaElement = document.createElement('textarea');
          const textareaElementContent = document.createTextNode(uriPath);

          textareaElement.classList.add('pseudo-hidden');
          textareaElement.appendChild(textareaElementContent);
          bodyElement.appendChild(textareaElement);

          const range = document.createRange();
          window.getSelection().removeAllRanges();
          range.selectNode(textareaElement);
          window.getSelection().addRange(range);

          try {
            document.execCommand('copy');
          } catch (error) {
            console.warn(error);
          }

          bodyElement.removeChild(textareaElement);
          window.getSelection().removeAllRanges();
        });
      });
    }
  }
}

new CopyToClipboard();
