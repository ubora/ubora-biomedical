export class Feedback {
    static initialize() {
        const userFeedback = document.querySelector('.js-feedback-input');
        const modalSendButton = document.querySelector('.js-feedback-send');

        if (modalSendButton) {
            modalSendButton.addEventListener('click', event => {
                const data = {
                    feedback: userFeedback.value,
                    fromPath: window.top.location.pathname
                };
                return this._sendFeedback(data);
            });
        }
    }

    static _closeModal() {
        const textarea = document.querySelector('.js-feedback-input');
        textarea.value = '';
        $('#feedbackModal').modal('hide');
    }

    static _openModal() {
        $('#feedbackModal').modal('toggle');
    }

    static _sendFeedback(data) {
        function createNotice(noticeTypeClass, stringMessage) {
            const noticeContainerElement = document.createElement('div');
            noticeContainerElement.classList.add('alert', noticeTypeClass, 'alert-dismissible', 'fade', 'show');
            noticeContainerElement.setAttribute('role', 'alert');

            const noticeCloseElement = document.createElement('button');
            noticeCloseElement.classList.add('close');
            noticeCloseElement.setAttribute('aria-label', 'Close');
            noticeCloseElement.setAttribute('data-dismiss', 'alert');
            const noticeCloseIconElement = document.createElement('span');
            noticeCloseIconElement.setAttribute('aria-hidden', 'true');
            const noticeCloseTextElement = document.createTextNode('×');
            noticeCloseIconElement.appendChild(noticeCloseTextElement);
            noticeCloseElement.appendChild(noticeCloseIconElement);



            const noticeMessageElement = document.createElement('p');
            const noticeMessageTextElement = document.createTextNode(stringMessage);
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
                createNotice('alert-success', 'Thank you for your feedback! 😃');
                return this._closeModal();
            }
        });
    }
}

Feedback.initialize()