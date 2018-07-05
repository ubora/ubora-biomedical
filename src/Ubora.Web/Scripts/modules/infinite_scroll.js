import InfiniteScroll from 'infinite-scroll'

global.UBORA.initInfiniteScroll = function (path) {
    return new InfiniteScroll('.infinite-scroll', {
        path: function () {
            var pageQueryRegex = /\page=[^&]+&*/;
            if (pageQueryRegex.test(path)) {
                var test = path.replace(pageQueryRegex, "page=" + (this.pageIndex + 1));
                return test;
            }
            return path + (path.indexOf('?') == -1 ? "?" : "&") + "page=" + (this.pageIndex + 1);
        },
        append: '.post',
        button: '.js-view-more-button',
        scrollThreshold: false,
        status: '.page-load-status',
        checkLastPage: '#js-more-pages-indicator'
    });
}