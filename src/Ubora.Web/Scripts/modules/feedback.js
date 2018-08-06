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
                modalSendButton.disabled = true;
                return this._sendFeedback(data);
            });
        }
    }

    static _closeModal() {
        const textarea = document.querySelector('.js-feedback-input');
        const modalSendButton = document.querySelector('.js-feedback-send');
        textarea.value = '';
        modalSendButton.disabled = false;
        $('#feedbackModal').modal('hide');
    }

    static _openModal() {
        $('#feedbackModal').modal('toggle');
    }

    static _sendFeedback(data) {
        $.ajax({
            url: `${window.top.location.origin}/Feedback/Send`,
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            headers: { 'RequestVerificationToken': document.getElementById('RequestVerificationToken').value },
            success: () => {
                $("#feedback-success").show();
                return this._closeModal();
            }
        });
    }
}

Feedback.initialize()