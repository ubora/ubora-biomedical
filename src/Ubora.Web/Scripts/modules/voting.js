export class Voting {
    constructor() {

        var setVotesStyle = function(element) {
            var children = element.children();
            var setAsSelected = false;

            for (var i = children.length - 1; i >= 0; i--) {
                var child = $(children[i]);

                if (child.find('input').is(':checked')) {
                    setAsSelected = true;
                }

                if (setAsSelected == true) {
                    child.find('label').addClass('voted');
                } else {
                    child.find('label').removeClass('voted');
                }

            }

        }

        $(".rating").each(function() {
            var voteContainer = $(this);

            voteContainer.children().click(function(element) {
                setVotesStyle(voteContainer);
            });


            setVotesStyle(voteContainer);
        });
    }
}

new Voting();