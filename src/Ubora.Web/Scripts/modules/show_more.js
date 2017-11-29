export default class ShowMoreText {
    static initialize() {

        $(document).ready(function () {

            console.log("asdf");

            const showChar = 300;
            const ellipsestext = "...";
            const moretext = "(show more)";
            const lesstext = "(show less)";

            $('.more').each(function () {
                const $this = $(this);

                const content = $this.text().trim();
                if (content.length > showChar) {

                    const textToShow = content.substr(0, showChar);
                    const textToHide = content.substr(showChar, content.length - showChar);

                    const html = textToShow + '<span class="more-ellipses">' + ellipsestext + '&nbsp;</span><span class="more-content"><span class="hidden-comment d-none">' + textToHide + '</span>&nbsp;&nbsp;<a href="" class="more-link">' + moretext + '</a></span>';

                    $this.html(html);
                }
            });

            $(".more-link").click(function () {
                const $this = $(this);

                const commentElement = $this.parent().parent();
                const hiddenComment = $this.prev();

                if ($this.hasClass("less")) {
                    $this.removeClass("less");
                    $this.html(moretext);
                    commentElement.find(".more-ellipses").removeClass("d-none");
                    hiddenComment.addClass('d-none');

                } else {
                    $this.addClass("less");
                    $this.html(lesstext);
                    commentElement.find(".more-ellipses").addClass("d-none");
                    hiddenComment.removeClass('d-none');
                }

                return false; // prevent default
            });
        });
    }
}

ShowMoreText.initialize();
