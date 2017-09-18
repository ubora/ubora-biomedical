const dismissNotice = userEvent => {
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

window.addEventListener('click', event => {
    dismissNotice(event);
});

window.addEventListener('touchend', event => {
    dismissNotice(event);
}, false);
