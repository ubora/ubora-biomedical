export class Sidemenu {
  constructor() {
    document
        .querySelectorAll('.js-sidemenu-toggle')
        .forEach(element => {
            element.addEventListener('click', this._toggleSidemenu);
            element.addEventListener('keyup', event => {
                const isEnterPressed = event.code === 'Enter';
                isEnterPressed ? this._toggleSidemenu() : undefined;
            });
        });
  }

  _toggleSidemenu() {
    return document
        .querySelector('.js-sidemenu')
        .classList
        .toggle('sidemenu-open');
  }
}

new Sidemenu();
