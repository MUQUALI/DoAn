

function initPagination(totalRecord, limit, displayPage, key) {
    var pagination = $(".pagination");
    var totalPage = Math.ceil(totalRecord / limit);
    var displayPage = totalPage > displayPage ? displayPage : totalPage;
    
    for (i = 0; i < displayPage; i++) {
        var page = i + 1;
        var li = $.parseHTML("<li class='page-item'><a class='page-link'>" + page +"</a></li>");

        if (i == 0) {
            li = $.parseHTML("<li class='page-item active'><a class='page-link'>" + page +"</a></li>");
        }
        pagination.append(li);
    }

    pageClick(totalPage, displayPage);
    getData(URL, 1, LIMIT, key);
}

function pageClick(totalPage, displayPage) {
    //$('.page-item').first().remove();
    $('.page-link').click(function () {
        //console.log($(this));
        $('.active').removeClass('active');
        $(this).parent().addClass('active');

        var curentPage = $(this).html();
        // set disable pre and next
        var lastPage = $('.page-item').last().children().html();
        var startPage = $('.page-item').first().children().html();

        var listPage = $('.page-item');

        if (curentPage == lastPage && curentPage < totalPage) {
            listPage.each(function (i, page) {
                page.children[0].innerHTML = parseInt(page.children[0].innerHTML) + 1;
            })
            // đổi lại class active cho page sau
            $(listPage[listPage.length - 2]).addClass('active');
            // remove class active ở page cuối
            $(listPage[listPage.length - 1]).removeClass('active');
        }

        if (curentPage == startPage && curentPage > 1) {
            listPage.each(function (i, page) {
                page.children[0].innerHTML = parseInt(page.children[0].innerHTML) - 1;
            });
            // đổi lại class active cho page sau
            $(listPage[0]).removeClass('active');
            // remove class active ở page cuối
            $(listPage[1]).addClass('active');
        }
        getData(URL, curentPage, LIMIT);
   })
}

function getData(url, page, limit, key) {
    $.ajax({
        type: 'GET',
        url: url,
        data: {
            page: page,
            limit: limit,
            key: key
        },
        success: data => { renderData(data) } // renderData bên AccountEvent.js
    })
}






