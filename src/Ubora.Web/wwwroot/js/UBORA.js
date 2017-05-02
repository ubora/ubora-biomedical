// Menu animation & controls - simple version for the time being
let headerElement = document.querySelector('.header');
let sideMenuButton = document.querySelector('.js-side-menu-control');
let sideMenu = document.querySelector('.js-side-menu');

sideMenuButton.addEventListener('click', () => {
    let menuVisibility = sideMenu.style.display;
    let headerHeight = headerElement.offsetHeight;

    sideMenu.style.top = `${headerHeight}px`;

    if (menuVisibility === 'block') {
        sideMenu.style.display = 'none';
    } else {
        sideMenu.style.display = 'block';
    }
});


window.addEventListener('load', () => {
    console.log(`Page loaded in ${window.performance.now()}ms`);
});
