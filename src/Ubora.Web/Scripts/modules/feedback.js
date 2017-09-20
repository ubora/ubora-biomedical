export class Feedback {
  constructor(triggerElement) {
    const modal = document.querySelector('.js-feedback-modal');
    const modalSendButton = document.querySelector('.js-feedback-send');
    const modalCloseButton = document.querySelector('.js-feedback-close');
    const userFeedback = document.querySelector('.js-feedback-input');

    triggerElement.addEventListener('click', event => {
      return this._openModal();
    });

    modalCloseButton.addEventListener('click', event => {
      return this._closeModal();
    });

    window.addEventListener('keyup', event => {
      if (event.key === 'Escape') {
        return this._closeModal();
      }
    });

    modalSendButton.addEventListener('click', event => {
      const data = {
        feedback: userFeedback.value,
        fromPath: window.top.location.pathname
      };
      return this._sendFeedback(data);
    });

    window.addEventListener('click', event => {
      if (event.path[0] === modal) {
        this._closeModal();
      }
    });
  }

  _closeModal() {
    const modal = document.querySelector('.js-feedback-modal');
    const textarea = document.querySelector('.js-feedback-input');

    textarea.value = '';
    modal.style.display = 'none';
  }

  _openModal() {
    const modal = document.querySelector('.js-feedback-modal');
    modal.style.display = 'block';
  }

  _sendFeedback(data) {
    $.ajax({
      url: `${window.top.location.origin}/Feedback/Send`,
      type: 'POST',
      data: JSON.stringify(data),
      contentType: 'application/json; charset=utf-8',
      success: () => {
        return this._closeModal();
      }
    });
  }
}

const feedbackButtonElement = document.querySelector('.js-feedback-trigger');
if (feedbackButtonElement) {
  new Feedback(feedbackButtonElement);
}
