'use strict';

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

    window.addEventListener('scroll', function (event) {
        if (document.body.scrollTop > headerHeight) {
            sideMenu.style.top = '0px';
        } else {
            sideMenu.style.top = totalPadding + 'px';
        }
    });
}

window.addEventListener('load', function () {
    // Target is to have this number in production setting < 500ms
    console.info('UBORA: page loaded in ' + Math.ceil(window.performance.now()) + 'ms');
});

// Markdown editor
marked.setOptions({
    renderer: new marked.Renderer(),
    gfm: true,
    tables: true,
    breaks: true,
    pedantic: false,
    sanitize: true,
    smartLists: true,
    smartypants: false
});

var markdownContainerArray = document.querySelectorAll('.text-markdown');
var markdownArrayHasItems = markdownContainerArray.length > 0;

if (markdownArrayHasItems) {
    for (var i = 0; i < markdownContainerArray.length; i++) {
        markdownContainerArray[i].innerHTML = marked(markdownContainerArray[i].innerHTML);
    }
}