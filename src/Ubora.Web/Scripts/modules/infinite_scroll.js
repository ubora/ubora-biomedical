﻿import InfiniteScroll from 'infinite-scroll'

global.UBORA.initInfiniteScroll = function (pageCount) {
    var infiniteScroll = new InfiniteScroll('.infinite-scroll', {
        path: function () {
            if (this.pageIndex < pageCount) {
                var nextIndex = this.pageIndex + 1;
                return '/UserList/Index?page=' + nextIndex;
            } else {
                document.querySelector('.view-more-button').style.display = 'none'
            }
        },
        append: '.post',
        button: '.view-more-button',
        scrollThreshold: false,
        status: '.page-load-status'
    });
}