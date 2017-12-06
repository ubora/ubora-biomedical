export default class ToggleExtraFields {
    static initialize() {

      $(function () {
          $(".show-extra-fields-js:checked, .toggle-extra-fields-js:checked").each(function() {
            toggleExtraFields(this, true);
          });

          $('.toggle-extra-fields-js')
              .change(function () {
                toggleExtraFields(this, $(this).is(':checked'));
              });

          $('.show-extra-fields-js')
              .click(function () {
                toggleExtraFields(this, true);
              });

          $('.hide-extra-fields-js')
              .click(function () {
                toggleExtraFields(this, false);
              });

          function toggleExtraFields(element, shouldShow) {
            var target = $(element).data('target');
            $(target).toggleClass('hide', !shouldShow);
          }
      });

    }
}

ToggleExtraFields.initialize();
