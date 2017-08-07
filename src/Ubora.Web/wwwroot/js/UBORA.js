// Menu animation & controls - simple version for the time being
var noticeElement = document.querySelector('.development-notice');
var headerElement = document.querySelector('.header');

var noticeHeight = noticeElement.offsetHeight;
var headerHeight = headerElement.offsetHeight;

var totalPadding = Math.ceil(noticeHeight + headerHeight);

var sideMenuButton = document.querySelector('.js-side-menu-control');
var sideMenu = document.querySelector('.js-side-menu');

if (sideMenuButton) {
    sideMenuButton.addEventListener('click', function () {
        var menuVisibility = sideMenu.style.display;

        sideMenu.style.top = totalPadding + 'px';

        if (menuVisibility === 'block') {
            sideMenu.style.display = 'none';
        } else {
            sideMenu.style.display = 'block';
        }
    });
}

window.addEventListener('load', function () {
    // Target is to have this number in production setting < 500ms
    console.info('UBORA: page loaded in ' + Math.ceil(window.performance.now()) + 'ms');
});

// Markdown editor
var renderer = new marked.Renderer();

marked.setOptions({
    renderer: renderer,
    gfm: true,
    tables: true,
    breaks: true,
    pedantic: false,
    sanitize: true,
    smartLists: true,
    smartypants: false
});

// Open link which redirects out of Ubora in new tab/window.
renderer.link = function (href, title, text) {
    return '<a target="_blank" href="' + href + '" title="' + title + '">' + text + '</a>';
};

// Picture size is optimized.
renderer.image = function (href, title) {
    return '<img class="image-markdown" src="' + href + ' " alt="' + title + '" />';
};

var markdownContainerArray = document.querySelectorAll('.text-markdown');
var markdownArrayHasItems = markdownContainerArray.length > 0;

if (markdownArrayHasItems) {
    for (var i = 0; i < markdownContainerArray.length; i++) {
        markdownContainerArray[i].innerHTML = marked(markdownContainerArray[i].innerHTML, { renderer: renderer });
    }
}

var simpleMde = new SimpleMDE({
    element: document.querySelector('.content_editable'), previewRender: function (plainText) {
        return marked(plainText, { renderer: renderer });
    }
});

// Feedback modal
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

// Notices
function dismissNotice(userEvent) {
  function removeParentElement(currentElement) {
    var parentElement = currentElement.parentElement;
    parentElement.remove();
  }

  var currentElement = userEvent.target;
  var notificationElement = currentElement.classList.contains('js-notice-close');

  if (notificationElement) {
    removeParentElement(currentElement);
  }
}

window.addEventListener('click', function (event) {
  dismissNotice(event);
});

window.addEventListener('touchend', function (event) {
  dismissNotice(event);
}, false);
