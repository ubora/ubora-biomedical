// Menu animation & controls - simple version for the time being
let noticeElement = document.querySelector('.development-notice');
let headerElement = document.querySelector('.header');

let noticeHeight = noticeElement.offsetHeight;
let headerHeight = headerElement.offsetHeight;

let totalPadding = Math.ceil(noticeHeight + headerHeight);

let sideMenuButton = document.querySelector('.js-side-menu-control');
let sideMenu = document.querySelector('.js-side-menu');

if (sideMenuButton) {
    sideMenuButton.addEventListener('click', () => {
        let menuVisibility = sideMenu.style.display;

        sideMenu.style.top = `${totalPadding}px`;

        if (menuVisibility === 'block') {
            sideMenu.style.display = 'none';
        } else {
            sideMenu.style.display = 'block';
        }
    });

    window.addEventListener('scroll', event => {
        if (document.body.scrollTop > headerHeight) {
            sideMenu.style.top = '0px';
        } else {
            sideMenu.style.top = `${totalPadding}px`;
        }
    });
}

window.addEventListener('load', () => {
    console.log(`Page loaded in ${window.performance.now()}ms`);
});
