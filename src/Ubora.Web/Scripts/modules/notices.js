export class Notices {
  constructor() {
    window.addEventListener('click', event => {
        this._dismissNotice(event);
    });

    window.addEventListener('touchend', event => {
        this._dismissNotice(event);
    }, false);
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
