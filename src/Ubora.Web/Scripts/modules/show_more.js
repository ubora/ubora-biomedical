export class ShowMoreText {
    constructor() {

      $(document).ready(function() {
        var showChar = 300;
        var ellipsestext = "...";
        var moretext = "(show more)";
        var lesstext = "(show less)";
        $('.more').each(function() {
          var content = $(this).text().trim();
          console.log(content, $(this))

          if(content.length > showChar) {

            var textToShow = content.substr(0, showChar);
            var textToHide = content.substr(showChar, content.length - showChar);

            var html = textToShow + '<span class="more-ellipses">' + ellipsestext+ '&nbsp;</span><span class="more-content"><span class="hidden-comment d-none">' + textToHide + '</span>&nbsp;&nbsp;<a href="" class="more-link">' + moretext + '</a></span>';

            $(this).html(html);
          }

        });

        $(".more-link").click(function(){

          var commentElement = $(this).parent().parent();
          var hiddenComment = $(this).prev();

          console.log($(this))

          if($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext);
            commentElement.find(".more-ellipses").removeClass("d-none");
            hiddenComment.addClass('d-none');

          } else {
            $(this).addClass("less");
            $(this).html(lesstext);
            commentElement.find(".more-ellipses").addClass("d-none");
            hiddenComment.removeClass('d-none');
          }

          return false;
        });
      });

    }
}

new ShowMoreText();
