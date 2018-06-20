import InfiniteScroll from 'infinite-scroll'

global.UBORA.initInfiniteScroll = function (path) {
    return new InfiniteScroll('.infinite-scroll', {
        path: function () {
            return path + "&page=" + (this.pageIndex + 1);
        },
        append: '.post',
        button: '.js-view-more-button',
        scrollThreshold: false,
        status: '.page-load-status',
        checkLastPage: '#js-more-pages-indicator'
    });
}