function feedbackModule(requestPath, sendFeedbackUrl) {
    function hideModal(event) {
        if (event.target === modal) {
            modal.style.display = 'none';
        }
    }

    var modal = document.getElementById('feedback-modal');
    var triggerButton = document.getElementById('feedback-modal-trigger');
    var closeButtton = document.getElementById('feedback-modal-close');

    triggerButton.onclick = function () {
        modal.style.display = 'block';
    };

    closeButtton.onclick = function () {
        modal.style.display = 'none';
    };

    window.addEventListener('touchend', hideModal, false);

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = hideModal;

    var sendButton = document.getElementById('feedback-send');
    var modalBeforeSend = document.getElementById('feedback-modal-before');
    var modalAfterSend = document.getElementById('feedback-modal-after');
    var userFeedback = document.getElementById('feedback');

    sendButton.onclick = (function () {
        var data = {
            feedback: userFeedback.value,
            fromPath: requestPath
        };

        $.ajax({
            url: sendFeedbackUrl,
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            success: function () {
                modalBeforeSend.style.display = 'none';
                modalAfterSend.style.display = '';
                userFeedback.value = '';

                window.setTimeout(function () {
                    modal.style.display = 'none';
                    modalBeforeSend.style.display = '';
                    modalAfterSend.style.display = 'none';
                }, 2000);
            }
        });
    });
}
