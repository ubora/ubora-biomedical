// TODO: Risto Kitsing - finish up what I started
const feedbackButtonElement = document.querySelector('.js-feedback-trigger');

if (feedbackButtonElement) {
  const feedbackSendButtonElement = document.querySelector('.js-feedback-send');
  const feedbackCancelButtonElement = document.querySelector('.js-feedback-cancel');
  const feedbackModalElement = document.querySelector('.js-feedback-modal');

  function toggleFeedbackModalVisibility() {
    return feedbackModalElement.classList.toggle('js-feedback-modal-invisible');
  }

  function sendFeedback() {
    const feedbackUrl = `${window.top.location.origin}/Feedback/Send`;
    const feedbackString = JSON.stringify(document.querySelector('.js-feedback-input').value);
    console.log(feedbackString);

    const options = {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json; charset=utf-8'
      },
      body: feedbackString
    };

    const sendAction = fetch(feedbackUrl, options).then(response => console.log(response));
  }

  feedbackButtonElement.addEventListener('click', event => {
    return toggleFeedbackModalVisibility();
  });

  feedbackCancelButtonElement.addEventListener('click', event => {
    return toggleFeedbackModalVisibility();
  });

  feedbackSendButtonElement.addEventListener('click', event => {
    sendFeedback();
  });

  window.addEventListener('keyup', event => {
    if (event.key === 'Escape') {
      return toggleFeedbackModalVisibility();
    }
  });
}
