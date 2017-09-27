export class Notices {
  constructor() {
    window.addEventListener('click', event => {
        this._dismissNotice(event);
    });

    window.addEventListener('touchend', event => {
        this._dismissNotice(event);
    }, false);
  }

  createNotice(noticeTypeClass, stringMessage) {
    const noticeContainerElement = document.createElement('div');
    noticeContainerElement.classList.add('notice', noticeTypeClass);

    const noticeCloseElement = document.createElement('span');
    const noticeCloseTextElement = document.createTextNode('Close');
    noticeCloseElement.classList.add('notice-close', 'js-notice-close');
    noticeCloseElement.appendChild(noticeCloseTextElement);

    const noticeMessageElement = document.createElement('p');
    const noticeMessageTextElement = document.createTextNode(stringMessage);
    noticeMessageElement.classList.add('notice-text');
    noticeMessageElement.appendChild(noticeMessageTextElement);

    noticeContainerElement.appendChild(noticeCloseElement);
    noticeContainerElement.appendChild(noticeMessageElement);

    document.querySelector('body').appendChild(noticeContainerElement);
  }

  _dismissNotice(userEvent) {
    const removeParentElement = currentElement => {
      const parentElement = currentElement.parentElement;
      return parentElement.remove();
    }

    const currentElement = userEvent.target;
    const notificationElement = currentElement.classList.contains('js-notice-close');

    if (notificationElement) {
      removeParentElement(currentElement)
    }
  }
}

new Notices();
