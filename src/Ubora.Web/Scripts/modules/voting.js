export default class Voting {
    static initialize() {

        const setVotesStyle = function(element) {
            const children = element.children();

            var setAsSelected = false;

            children.forEach(child => {
                if (child.find('input').is(':checked')) {
                    setAsSelected = true;
                }

                if (setAsSelected == true) {
                    child.find('label').addClass('voted');
                } else {
                    child.find('label').removeClass('voted');
                }
            });
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

Voting.initialize();