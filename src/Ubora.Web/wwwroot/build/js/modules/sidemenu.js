module.exports = sidemenu = () => {
  const toggleSideMenu = () => {
      return document
          .querySelector('.js-sidemenu')
          .classList
          .toggle('sidemenu-open');
  };

  document
      .querySelectorAll('.js-sidemenu-toggle')
      .forEach(element => {
          element.addEventListener('click', toggleSideMenu);
          element.addEventListener('keyup', event => {
              const isEnterPressed = event.code === 'Enter';
              isEnterPressed ? toggleSideMenu() : undefined;
          });
      });
};
