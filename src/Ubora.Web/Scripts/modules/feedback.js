export class Feedback {
    constructor(triggerElement) {
        const modal = document.querySelector('.js-feedback-modal');
        const modalSendButton = document.querySelector('.js-feedback-send');
        const modalCloseButton = document.querySelector('.js-feedback-close');
        const modalCancelButton = document.querySelector('.js-feedback-cancel');
        const userFeedback = document.querySelector('.js-feedback-input');

        triggerElement.addEventListener('click', event => {
            return this._openModal();
        });

        modalCloseButton.addEventListener('click', event => {
            return this._closeModal();
        });

        modalCancelButton.addEventListener('click', event => {
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
        const textarea = document.querySelector('.js-feedback-input');
        textarea.value = '';
        $('#feedbackModal').modal('hide');
    }

    _openModal() {
        $('#feedbackModal').modal('toggle');
    }

    _sendFeedback(data) {
        function createNotice(noticeTypeClass, stringMessage) {
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

            return document.querySelector('body').appendChild(noticeContainerElement);
        }

        $.ajax({
            url: `${window.top.location.origin}/Feedback/Send`,
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            success: () => {
                createNotice('notice-success', 'Thank you for your feedback! 😃');
                return this._closeModal();
            }
        });
    }
}

const feedbackButtonElement = document.querySelector('.js-feedback-trigger');
if (feedbackButtonElement) {
    new Feedback(feedbackButtonElement);
}
