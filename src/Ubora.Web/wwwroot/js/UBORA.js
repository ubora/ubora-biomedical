// Menu animation & controls - simple version for the time being
let headerElement = document.querySelector('.header');
let headerHeight = headerElement.offsetHeight;

let sideMenuButton = document.querySelector('.js-side-menu-control');
let sideMenu = document.querySelector('.js-side-menu');

if (sideMenuButton) {
    sideMenuButton.addEventListener('click', () => {
        let menuVisibility = sideMenu.style.display;

        sideMenu.style.top = `${headerHeight}px`;

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
            sideMenu.style.top = `${headerHeight}px`;
        }
    });
}

window.addEventListener('load', () => {
    console.log(`Page loaded in ${window.performance.now()}ms`);
});
