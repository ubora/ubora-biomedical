export default class Voting {
    static initialize() {

        const setVotesStyle = function (element) {
            const children = element.children();
            var setAsSelected = false;

            // Iterate the children in reverse to addd 'voted' class in order from 1 to 5.
            for (let i = children.length - 1; i >= 0; i--) {
                const child = $(children[i]);

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

        $(".rating").each(function () {
            var voteContainer = $(this);

            voteContainer.children().click(function (element) {
                setVotesStyle(voteContainer);
            });


            setVotesStyle(voteContainer);
        });
    }
}

Voting.initialize();