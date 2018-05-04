import InfiniteScroll from 'infinite-scroll'

global.UBORA.initInfiniteScroll = function (path, pageCount, title) {
    var infiniteScroll = new InfiniteScroll('.infinite-scroll', {
        path: function () {
            if (this.pageIndex < pageCount && title !== undefined) {
                var nextIndex = this.pageIndex + 1;
                return '/ProjectList/Search/?title=' + title + path + nextIndex;
            } else if (this.pageIndex < pageCount && title === undefined) {
                var nextIndex = this.pageIndex + 1;
                return path + nextIndex;
            }
            else {
                document.querySelector('.view-more-button').style.display = 'none'
            }
        },
        append: '.post',
        button: '.view-more-button',
        scrollThreshold: false,
        status: '.page-load-status'
    });
}