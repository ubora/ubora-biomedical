export class Sidemenu {
  constructor() {
    const sidemenuToggleElementList = document.querySelectorAll('.js-sidemenu-toggle');

    for (let i = 0; i < sidemenuToggleElementList.length; i++) {
      let element = sidemenuToggleElementList[i];

      element.addEventListener('click', this._toggleSidemenu);
      element.addEventListener('keyup', event => {
        const isEnterPressed = event.code === 'Enter';
        isEnterPressed ? this._toggleSidemenu() : undefined;
      });
    }
  }

  _toggleSidemenu() {
    return document
        .querySelector('.js-sidemenu')
        .classList
        .toggle('sidemenu-open');
  }
}

new Sidemenu();
